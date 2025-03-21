using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public class Boolean : JSObject
{
    internal const string TrueString = "true";
    internal const string FalseString = "false";

    [Hidden] internal static readonly Boolean True = new(true) { _attributes = JSValueAttributesInternal.SystemObject };

    [Hidden] internal static readonly Boolean False = new(false)
        { _attributes = JSValueAttributesInternal.SystemObject };

    [DoNotEnumerate]
    public Boolean()
    {
        _valueType = JSValueType.Boolean;
        _iValue = 0;
        _attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(Arguments obj)
    {
        if (obj == null)
            throw new ArgumentNullException("obj");
        _valueType = JSValueType.Boolean;
        _iValue = (bool)obj[0] ? 1 : 0;
        _attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(bool value)
    {
        _valueType = JSValueType.Boolean;
        _iValue = value ? 1 : 0;
        _attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(double value)
    {
        _valueType = JSValueType.Boolean;
        _iValue = value != 0 && !double.IsNaN(value) ? 1 : 0;
        _attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(int value)
    {
        _valueType = JSValueType.Boolean;
        _iValue = value != 0 ? 1 : 0;
        _attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(string value)
    {
        _valueType = JSValueType.Boolean;
        _iValue = !string.IsNullOrEmpty(value) ? 1 : 0;
        _attributes |= JSValueAttributesInternal.SystemObject;
    }

#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [Hidden]
    public static implicit operator Boolean(bool value)
    {
#if DEBUG
        if (True._iValue != 1)
            Debugger.Break();
        if (False._iValue != 0)
            Debugger.Break();
#endif
        return value ? True : False;
        //var res = value ? Boolean.True : Boolean.False;
        //res.iValue = value ? 1 : 0;
        //return res;
    }

    [Hidden]
    public static implicit operator bool(Boolean value)
    {
        return value != null && value._iValue != 0;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue toLocaleString(JSValue self)
    {
        if (self.GetType() != typeof(Boolean) && self._valueType != JSValueType.Boolean)
            ExceptionHelper.Throw(new TypeError("Boolean.prototype.toLocaleString called for not boolean."));
        return self._iValue != 0 ? "true" : "false";
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue valueOf(JSValue self)
    {
        if (self.GetType() == typeof(Boolean))
            return self._iValue != 0;
        if (self._valueType != JSValueType.Boolean)
            ExceptionHelper.Throw(new TypeError("Boolean.prototype.valueOf called for not boolean."));
        return self;
    }

    [CLSCompliant(false)]
    [InstanceMember]
    [ArgumentsCount(0)]
    [DoNotEnumerate]
    public static JSValue toString(JSValue self, Arguments args)
    {
        if (self.GetType() != typeof(Boolean) && self._valueType != JSValueType.Boolean)
            ExceptionHelper.Throw(new TypeError("Boolean.prototype.toString called for not boolean."));
        return self._iValue != 0 ? "true" : "false";
    }
}