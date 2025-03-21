﻿using System;
using System.Collections;
using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js;

public sealed class ExportTable : IEnumerable<KeyValuePair<string, JSValue>>
{
    private readonly Context _context;
    private readonly Dictionary<string, JSValue> _items = new();


    public ExportTable(Context moduleContext)
    {
        _context = moduleContext;
    }

    public JSValue this[string key]
    {
        get
        {
            if (string.IsNullOrWhiteSpace(key) || !Parser.ValidateName(key, 0, false, true, false))
                ExceptionHelper.Throw(new ArgumentException());

            var result = JSValue.undefined;

            if (!_items.TryGetValue(key, out result))
                return JSValue.undefined;

            return result;
        }
        set => _items[key] = value;
    }

    public int Count => _items.Count;

    public JSValue Default
    {
        get
        {
            var result = JSValue.undefined;

            if (!_items.TryGetValue("", out result))
                return JSValue.undefined;

            return result;
        }
    }

    public IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///     Add object constructor to export
    /// </summary>
    /// <param name="type">Class</param>
    /// <param name="name"></param>
    public void AddConstructor(Type type, string name)
    {
        var ctor = _context.GlobalContext.GetConstructor(type);
        _items.Add(name, ctor);
    }

    /// <summary>
    ///     Add object to exports
    /// </summary>
    /// <param name="name">Name of variable in export</param>
    /// <param name="deletable"></param>
    public JSValue AddVariable(string name, bool deletable)
    {
        var defineVariable = new JSValue
        {
            _valueType = JSValueType.Undefined
        };
        _items.Add(name, defineVariable);
        return defineVariable;
    }

    /// <summary>
    ///     Add readonly object to exports
    /// </summary>
    /// <param name="name">Name of variable in export</param>
    /// <param name="value">JSValue (function, constructor, object, etc)</param>
    /// <param name="deletable"></param>
    public JSValue AddConstant(string name, JSValue value, bool deletable = false)
    {
        var v = AddVariable(name, deletable);
        v.Assign(value);
        v._attributes |= JSValueAttributesInternal.ReadOnly;
        return v;
    }

    public JSObject CreateExportList()
    {
        var result = JSObject.CreateObject(true);

        foreach (var item in _items)
            if (item.Key != "")
                result._fields[item.Key] = item.Value;
            else
                result._fields["default"] = item.Value;

        return result;
    }
}