using System.Collections.Generic;
using System.Linq;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;
using Totletheyn.Core.Js.Extensions;

namespace Totletheyn.Core.Js.BaseLibrary;

[RequireNewKeyword]
public sealed class Map : IIterable
{
    private readonly Dictionary<object, object> _storage;

    public Map()
    {
        _storage = new Dictionary<object, object>();
    }

    public Map(IIterable iterable)
        : this()
    {
        if (iterable == null)
            return;

        foreach (var item in iterable.AsEnumerable())
        {
            if (item._valueType < JSValueType.Object)
                ExceptionHelper.ThrowTypeError($"Iterator value {item} is not an entry object");

            var value = item["1"];
            var key = item["0"].Value;
            _storage[key] = value.Value as JSValue ?? value;
        }
    }

    public int size => _storage.Count;

    public IIterator iterator()
    {
        var globalContext = Context.CurrentGlobalContext;
        return _storage
            .Select(x => new Array { globalContext.ProxyValue(x.Key), globalContext.ProxyValue(x.Value) })
            .GetEnumerator()
            .AsIterator();
    }

    public object get(object key)
    {
        if (key == null)
            key = JSValue.@null;
        else
            key = (key as JSValue)?.Value ?? key;

        _storage.TryGetValue(key, out var result);
        return result;
    }

    public Map set(object key, object value)
    {
        if (key == null)
            key = JSValue.@null;
        else
            key = (key as JSValue)?.Value ?? key;

        _storage[key] = value;

        return this;
    }

    public void clear()
    {
        _storage.Clear();
    }

    public bool delete(object key)
    {
        if (key == null)
            key = JSValue.@null;
        else
            key = (key as JSValue)?.Value ?? key;

        return _storage.Remove(key);
    }

    public bool has(object key)
    {
        if (key == null)
            key = JSValue.@null;
        else
            key = (key as JSValue)?.Value ?? key;

        return _storage.ContainsKey(key);
    }

    public void forEach(Function callback, JSValue thisArg)
    {
        foreach (var item in _storage) callback.Call(thisArg, new Arguments { item.Value, item.Key, this });
    }

    public IIterator keys()
    {
        return _storage.Keys.AsIterable().iterator();
    }

    public IIterator values()
    {
        return _storage.Values.AsIterable().iterator();
    }

    public IIterator entries()
    {
        return iterator();
    }
}