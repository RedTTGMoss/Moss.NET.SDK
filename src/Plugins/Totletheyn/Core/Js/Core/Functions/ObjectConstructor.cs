﻿using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.Core.Functions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[Prototype(typeof(Function), true)]
internal class ObjectConstructor : ConstructorProxy
{
    public ObjectConstructor(Context context, StaticProxy staticProxy, JSObject prototype)
        : base(context, staticProxy, prototype)
    {
        _length = new Number(1);
    }

    public override string name => "Object";

    protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
    {
        var nestedValue = targetObject;
        if (nestedValue != null && (nestedValue._attributes & JSValueAttributesInternal.ConstructingObject) == 0)
            nestedValue = null;

        if (arguments != null && arguments._iValue > 0)
            nestedValue = arguments[0];

        if (nestedValue == null)
            return ConstructObject();

        if (nestedValue._valueType >= JSValueType.Object)
        {
            if (nestedValue._oValue == null)
                return ConstructObject();

            return nestedValue;
        }

        if (nestedValue._valueType <= JSValueType.Undefined)
            return ConstructObject();

        return nestedValue.ToObject();
    }

    protected internal override JSValue ConstructObject()
    {
        return CreateObject();
    }

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(bool hideNonEnum,
        EnumerationMode enumerationMode, PropertyScope propertyScope = PropertyScope.Common)
    {
        var pe = _staticProxy.GetEnumerator(hideNonEnum, enumerationMode, propertyScope);
        while (pe.MoveNext())
            yield return pe.Current;

        if (propertyScope is not PropertyScope.Own)
        {
            pe = __proto__.GetEnumerator(hideNonEnum, enumerationMode, PropertyScopeForProto(propertyScope));
            while (pe.MoveNext())
                yield return pe.Current;
        }
    }
}