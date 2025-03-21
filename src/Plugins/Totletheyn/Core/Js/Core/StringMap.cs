﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Totletheyn.Core.Js.Core;

internal sealed class StringMapDebugView<TValue>
{
    private StringMap<TValue> stringMap;

    public StringMapDebugView(StringMap<TValue> stringMap)
    {
        this.stringMap = stringMap;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public KeyValuePair<string, TValue>[] Items => new List<KeyValuePair<string, TValue>>(stringMap).ToArray();
}

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(StringMapDebugView<>))]
public class StringMap<TValue> : IDictionary<string, TValue>
{
    private const int InitialSize = 4;
    private const int MaxAsListSize = 4;
    private readonly object _sync = new();
    private int _eicount;
    private int[] _existsedIndexes;
    private int _previousIndex;

    private Record[] _records = EmpryArrayHelper.Empty<Record>();

    private int _version;

    public StringMap()
    {
        Clear();
    }

    public bool TryGetValue(string key, out TValue value)
    {
        if (key == null)
            throw new ArgumentNullException("key");

        var previousIndex = _previousIndex;
        var records = _records;
        if (records.Length <= MaxAsListSize)
        {
            for (var i = 0; i < records.Length; i++)
            {
                ref var record = ref records[i];
                if (record.key == null)
                    break;

                if (string.CompareOrdinal(record.key, key) == 0)
                {
                    value = record.value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        if (previousIndex != -1 && string.CompareOrdinal(records[previousIndex].key, key) == 0)
        {
            value = records[previousIndex].value;
            return true;
        }

        var rcount = records.Length;

        if (rcount == 0)
        {
            value = default;
            return false;
        }

        var hash = computeHash(key);
        var index = hash & (rcount - 1);

        do
        {
            if (records[index].hash == hash
                && records[index].key is not null
                && string.CompareOrdinal(records[index].key, key) == 0)
            {
                value = records[index].value;
                _previousIndex = index;
                return true;
            }

            index = records[index].next - 1;
        } while (index >= 0);

        value = default;
        return false;
    }

    public bool Remove(string key)
    {
        if (key == null)
            throw new ArgumentNullException();

        var records = _records;

        if (records.Length == 0)
            return false;

        if (records.Length <= MaxAsListSize)
        {
            var found = false;
            try
            {
                for (var i = 0; i < records.Length; i++)
                    if (found)
                    {
                        records[i - 1].key = records[i].key;
                        records[i - 1].value = records[i].value;
                        records[i].key = null;
                        records[i].value = default;
                    }
                    else if (string.CompareOrdinal(records[i].key, key) == 0)
                    {
                        Monitor.Enter(_sync);

                        if (records != _records)
                            return Remove(key);

                        Count--;
                        _eicount--;
                        _version++;
                        found = true;
                        records[i].key = null;
                        records[i].value = default;
                    }
            }
            finally
            {
                if (found)
                    Monitor.Exit(_sync);
            }

            return found;
        }

        var mask = records.Length - 1;
        var hash = computeHash(key);
        int index;
        var prevItemIndex = -1;

        for (index = hash & mask; index >= 0; index = records[index].next - 1)
        {
            if (records[index].hash == hash
                && records[index].key is not null
                && string.CompareOrdinal(records[index].key, key) == 0)
                lock (_sync)
                {
                    if (records != _records)
                        return Remove(key);

                    if (index == _previousIndex)
                        _previousIndex = -1;

                    records[index].key = null;
                    records[index].value = default;
                    records[index].hash = 0;

                    if (prevItemIndex >= 0 && records[index].next == 0)
                        records[prevItemIndex].next = 0;

                    var indexInExIndex = Array.IndexOf(_existsedIndexes, index);
                    Array.Copy(_existsedIndexes, indexInExIndex + 1, _existsedIndexes, indexInExIndex,
                        _existsedIndexes.Length - indexInExIndex - 1);

                    Count--;
                    _eicount--;
                    _version++;

                    return true;
                }

            prevItemIndex = index;
        }

        return false;
    }

    public void Add(string key, TValue value)
    {
        lock (_records)
        {
            insert(key, value, computeHash(key), true, true);
        }
    }

    public bool ContainsKey(string key)
    {
        TValue fake;
        return TryGetValue(key, out fake);
    }

    public ICollection<string> Keys => (from item in _records where item.key != null select item.key).ToArray();

    public ICollection<TValue> Values => (from item in _records where item.key != null select item.value).ToArray();

    public TValue this[string key]
    {
        get
        {
            TValue result;
            if (!TryGetValue(key, out result))
                throw new KeyNotFoundException();
            return result;
        }
        set => insert(key, value, computeHash(key), false, true);
    }

    public virtual void Add(KeyValuePair<string, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        if (_existsedIndexes != null)
            Array.Clear(_existsedIndexes, 0, _existsedIndexes.Length);
        Array.Clear(_records, 0, _records.Length);
        Count = 0;
        _eicount = 0;
        _version++;
        _previousIndex = -1;
    }

    public virtual bool Contains(KeyValuePair<string, TValue> item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
    {
        foreach (var item in this)
            if (arrayIndex < array.Length)
                array[arrayIndex++] = item;
            else
                break;
    }

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public virtual bool Remove(KeyValuePair<string, TValue> item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        uint exprected = 0;

        var yielded = false;
        do
        {
            yielded = false;
            var prevVersion = _version;

            var minDelta = uint.MaxValue;
            var indexOfMinDelta = -1;

            for (var i = 0; i < _records.Length; i++)
            {
                var index = i;
                if (_records[index].key != null
                    && uint.TryParse(_records[index].key, NumberStyles.Integer, CultureInfo.InvariantCulture,
                        out var number))
                {
                    if (exprected == number)
                    {
                        yield return new KeyValuePair<string, TValue>(_records[index].key, _records[index].value);
                        exprected++;
                        yielded = true;
                        minDelta = uint.MinValue;
                        indexOfMinDelta = -1;
                    }
                    else if (number > exprected &&
                             (number - exprected < minDelta || (exprected == 0 && number == uint.MaxValue)))
                    {
                        minDelta = number - exprected;
                        indexOfMinDelta = index;
                    }
                }
            }

            if (indexOfMinDelta >= 0)
            {
                yield return new KeyValuePair<string, TValue>(_records[indexOfMinDelta].key,
                    _records[indexOfMinDelta].value);

                if (exprected + minDelta == uint.MaxValue)
                    break;

                exprected += minDelta + 1;
                yielded = true;
            }
        } while (yielded);

        for (var i = 0; i < _eicount; i++)
        {
            var index = _existsedIndexes[i];
            if (_records[index].key != null
                && !uint.TryParse(_records[index].key, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                yield return new KeyValuePair<string, TValue>(_records[index].key, _records[index].value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void insert(string key, TValue value, int hash, bool @throw, bool allowIncrease)
    {
        if (key == null)
            ExceptionHelper.ThrowArgumentNull("key");

        int index;
        var colisionCount = 0;
        var records = _records;
        var mask = records.Length - 1;
        if (records.Length == 0)
            lock (_sync)
            {
                if (records == _records)
                    mask = increaseSize() - 1;
                else
                    mask = _records.Length - 1;

                records = _records;
            }

        if (records.Length <= MaxAsListSize)
        {
            for (var i = 0; i < records.Length; i++)
            {
                if (records[i].key == null)
                    lock (_sync)
                    {
                        if (records != _records)
                        {
                            insert(key, value, hash, @throw, allowIncrease);
                            return;
                        }

                        if (records[i].key == null)
                        {
                            records[i].key = key;
                            records[i].hash = -1;
                            records[i].value = value;

                            ensureExistedIndexCapacity();
                            _existsedIndexes[_eicount] = i;

                            Count++;
                            _eicount++;
                            _version++;
                            return;
                        }
                    }

                if (string.CompareOrdinal(records[i].key, key) == 0)
                {
                    if (@throw)
                        ExceptionHelper.Throw(new InvalidOperationException("Item already exists"));

                    records[i].value = value;
                    return;
                }
            }

            if (records.Length * 2 <= MaxAsListSize)
                lock (_sync)
                {
                    if (records != _records)
                    {
                        insert(key, value, hash, @throw, allowIncrease);
                        return;
                    }

                    index = records.Length;
                    increaseSize();
                    records = _records;

                    records[index].hash = -1;
                    records[index].key = key;
                    records[index].value = value;

                    ensureExistedIndexCapacity();
                    _existsedIndexes[_eicount] = index;

                    Count++;
                    _eicount++;
                    _version++;
                    return;
                }
        }
        else
        {
            index = hash & mask;
            do
            {
                if (records[index].hash == hash
                    && records[index].key is not null
                    && string.CompareOrdinal(records[index].key, key) == 0)
                {
                    if (@throw)
                        ExceptionHelper.Throw(new InvalidOperationException("Item already Exists"));

                    lock (_sync)
                    {
                        if (records != _records)
                        {
                            insert(key, value, hash, @throw, allowIncrease);
                            return;
                        }

                        records[index].value = value;
                        return;
                    }
                }

                index = records[index].next - 1;
            } while (index >= 0);
        }

        // не нашли

        if (allowIncrease)
            if (Count == mask + 1
                || (Count > 50 && Count * 6 / 5 >= mask))
                lock (_sync)
                {
                    if (records != _records)
                    {
                        insert(key, value, hash, @throw, allowIncrease);
                        return;
                    }

                    if (Count == mask + 1
                        || (Count > 50 && Count * 6 / 5 >= mask))
                    {
                        mask = increaseSize() - 1;
                        records = _records;
                    }
                }

        index = hash & mask;
        var prevIndex = index;

        while (records[index].key is not null && records[index].next > 0)
        {
            prevIndex = index;
            index = records[index].next - 1;
            colisionCount++;
        }

        if (records[index].key is not null)
        {
            prevIndex = index;
            do
            {
                index = (index + 17) & mask;
                if (index == prevIndex || records != _records)
                {
                    insert(key, value, hash, @throw, allowIncrease);
                    return;
                }
            } while (records[index].key is not null);
        }

        lock (_sync)
        {
            ref var record = ref records[index];
            if (records != _records
                || record.key is not null
                || (prevIndex != index
                    && records[prevIndex].next != 0
                    && records[prevIndex].next - 1 != index))
            {
                insert(key, value, hash, @throw, allowIncrease);
                return;
            }

            record.hash = hash;
            record.key = key;
            record.value = value;

            if (prevIndex >= 0 && index != prevIndex)
                records[prevIndex].next = index + 1;

            ensureExistedIndexCapacity();
            _existsedIndexes[_eicount] = index;

            _eicount++;
            Count++;
            _version++;

            if (colisionCount > 17 && allowIncrease && _eicount * 10 > records.Length)
                increaseSize();
        }
    }

    private void ensureExistedIndexCapacity()
    {
        if (_eicount == _existsedIndexes.Length)
        {
            // Увеличиваем размер массива с занятыми номерами
            var newEI = new int[_existsedIndexes.Length << 1];
            Array.Copy(_existsedIndexes, newEI, _existsedIndexes.Length);
            _existsedIndexes = newEI;
        }
    }

    private static int computeHash(string key)
    {
        unchecked
        {
            int hash;
            var keyLen = key.Length;
            var m = (keyLen >> 2) | 4;
            hash = (int)((uint)keyLen * 0x30303) ^ 0xb7b7b7;
            if (keyLen > 0)
                for (var i = 0; i < m; i++)
                    hash = (hash >> 14)
                           ^ (hash * (int)0xf351_f351)
                           ^ (key[i % keyLen] * 0x34df_5981);

            return hash;
        }
    }

    private int increaseSize()
    {
        if (_records.Length == 0)
        {
            _records = new Record[InitialSize];
            _existsedIndexes = new int[InitialSize];
        }
        else
        {
            var oldRecords = _records;

            if (oldRecords.Length > 1 << 20)
                throw new Exception("My exception");

            var newLength = _records.Length << 1;
            _records = new Record[newLength];

            var c = _eicount;
            Count = 0;
            _eicount = 0;

            if (oldRecords.Length == MaxAsListSize)
                for (var i = 0; i < c; i++)
                {
                    var index = _existsedIndexes[i];
                    ref var record = ref oldRecords[index];
                    if (record.key != null)
                        record.hash = computeHash(record.key);
                }

            for (var i = 0; i < c; i++)
            {
                var index = _existsedIndexes[i];
                ref var record = ref oldRecords[index];
                if (record.key != null)
                    insert(record.key, record.value, record.hash, false, false);
            }
        }

        _previousIndex = -1;
        return _records.Length;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Record
    {
        // Порядок полей не менять!
        public int hash;
        public string key;
        public int next;
        public TValue value;
#if DEBUG
        public override string ToString()
        {
            return "[" + key + ", " + value + "]";
        }
#endif
    }
}