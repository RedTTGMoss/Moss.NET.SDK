using System;
using System.Collections;
using System.Collections.Generic;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.Extensions;

internal abstract class IterableProtocolBase
{
    [Hidden]
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    [Hidden]
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    [Hidden]
    public override string ToString()
    {
        return base.ToString();
    }
}

internal sealed class EnumeratorResult : IterableProtocolBase, IIteratorResult
{
    [Hidden]
    public EnumeratorResult(bool done, JSValue value)
    {
        this.value = value;
        this.done = done;
    }

    public JSValue value { get; }

    public bool done { get; }
}

internal sealed class EnumeratorToIteratorWrapper : IterableProtocolBase, IIterator, IIterable
{
    private readonly GlobalContext _context;
    private readonly IEnumerator _enumerator;

    [Hidden]
    public EnumeratorToIteratorWrapper(IEnumerator enumerator)
    {
        _enumerator = enumerator;
        _context = Context.CurrentGlobalContext;
    }

    public IIterator iterator()
    {
        return this;
    }

    public IIteratorResult next(Arguments arguments = null)
    {
        var read = _enumerator.MoveNext();
        return new EnumeratorResult(
            !read,
            _context.ProxyValue(read ? _enumerator.Current : null));
    }

    public IIteratorResult @return()
    {
        return new EnumeratorResult(true, null);
    }

    public IIteratorResult @throw(Arguments arguments = null)
    {
        return new EnumeratorResult(true, null);
    }
}

internal sealed class EnumerableToIterableWrapper : IterableProtocolBase, IIterable
{
    private readonly IEnumerable enumerable;

    [Hidden]
    public EnumerableToIterableWrapper(IEnumerable enumerable)
    {
        this.enumerable = enumerable;
    }

    public IIterator iterator()
    {
        return new EnumeratorToIteratorWrapper(enumerable.GetEnumerator());
    }
}

internal sealed class IteratorItemAdapter : IterableProtocolBase, IIteratorResult
{
    private readonly JSValue result;

    [Hidden]
    public IteratorItemAdapter(JSValue result)
    {
        this.result = result;
    }

    public JSValue value => Tools.GetPropertyOrValue(result["value"], result);

    public bool done => (bool)Tools.GetPropertyOrValue(result["done"], result);
}

internal sealed class IteratorAdapter : IterableProtocolBase, IIterator, IIterable
{
    private readonly JSValue iterator;

    [Hidden]
    public IteratorAdapter(JSValue iterator)
    {
        this.iterator = iterator;
    }

    IIterator IIterable.iterator()
    {
        return this;
    }

    public IIteratorResult next(Arguments arguments = null)
    {
        var result = iterator["next"].As<Function>().Call(iterator, arguments);
        return new IteratorItemAdapter(result);
    }

    public IIteratorResult @return()
    {
        var result = iterator["return"].As<Function>().Call(iterator, null);
        return new IteratorItemAdapter(result);
    }

    public IIteratorResult @throw(Arguments arguments = null)
    {
        var result = iterator["throw"].As<Function>().Call(iterator, null);
        return new IteratorItemAdapter(result);
    }
}

internal sealed class IterableAdapter : IterableProtocolBase, IIterable
{
    private readonly JSValue source;

    [Hidden]
    public IterableAdapter(JSValue source)
    {
        this.source = source.IsBox ? source._oValue as JSValue : source;
    }

    public IIterator iterator()
    {
        var iteratorFunction = source.GetProperty(Symbol.iterator, false, PropertyScope.Common);
        if (iteratorFunction._valueType != JSValueType.Function)
            return null;

        var iterator = iteratorFunction.As<Function>().Call(source, null);
        if (iterator == null)
            return null;

        return new IteratorAdapter(iterator);
    }
}

public static class IterationProtocolExtensions
{
    public static IEnumerable<JSValue> AsEnumerable(this IIterable iterableObject)
    {
        var iterator = iterableObject.iterator();
        if (iterator == null)
            yield break;

        var item = iterator.next();
        while (!item.done)
        {
            yield return item.value;
            item = iterator.next();
        }
    }

    public static IIterable AsIterable(this JSValue source)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        return source.Value as IIterable ?? new IterableAdapter(source);
    }

    public static bool IsIterable(this JSValue source)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        return source.Value is IIterable ||
               source.GetProperty(Symbol.iterator, false, PropertyScope.Common)._valueType == JSValueType.Function;
    }

    public static IIterable AsIterable(this IEnumerable enumerable)
    {
        return new EnumerableToIterableWrapper(enumerable);
    }

    public static IIterator AsIterator(this IEnumerator enumerator)
    {
        return new EnumeratorToIteratorWrapper(enumerator);
    }
}