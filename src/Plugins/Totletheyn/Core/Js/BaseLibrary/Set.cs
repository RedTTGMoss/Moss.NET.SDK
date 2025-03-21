﻿using System.Collections.Generic;
using System.Linq;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;
using Totletheyn.Core.Js.Extensions;

namespace Totletheyn.Core.Js.BaseLibrary;

[RequireNewKeyword]
public sealed class Set : IIterable
{
    private readonly HashSet<object> _storage;

    public Set()
    {
        _storage = new HashSet<object>();
    }

    public Set(IIterable iterable)
        : this()
    {
        if (iterable == null)
            return;

        foreach (var value in iterable.AsEnumerable()) _storage.Add(value.Value);
    }

    public int size
    {
        [Hidden] get => _storage.Count;
    }

    public IIterator iterator()
    {
        return _storage
            .GetEnumerator()
            .AsIterator();
    }

    public Set add(object item)
    {
        if (item == null)
            item = JSValue.@null;
        else
            item = (item as JSValue)?.Value ?? item;
        _storage.Add(item);

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
        return _storage.Contains(key);
    }

    public void forEach(Function callback, JSValue thisArg)
    {
        foreach (var item in _storage)
        {
            var args = new Arguments { item, null, this };
            args[1] = args[0];
            callback.Call(thisArg, args);
        }
    }

    public IIterator keys()
    {
        return _storage.AsIterable().iterator();
    }

    public IIterator values()
    {
        return _storage.AsIterable().iterator();
    }

    public IIterator entries()
    {
        var globalContext = Context.CurrentGlobalContext;
        return _storage
            .Select(x =>
            {
                var value = globalContext.ProxyValue(x);
                return new Array { value, value };
            })
            .AsIterable()
            .iterator();
    }
}