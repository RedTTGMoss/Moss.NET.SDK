using System;
using Totletheyn.Core.Js.Core;
using Boolean = Totletheyn.Core.Js.BaseLibrary.Boolean;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public class Equal : Expression
{
    public Equal(Expression first, Expression second)
        : base(first, second, false)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public override JSValue Evaluate(Context context)
    {
        var temp = _left.Evaluate(context);
        JSValue tjso;
        int tint;
        double tdouble;
        string tstr;
        var index = 0;
        switch (temp._valueType)
        {
            case JSValueType.Boolean:
            case JSValueType.Integer:
            {
                tint = temp._iValue;
                tjso = _right.Evaluate(context);
                switch (tjso._valueType)
                {
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                    {
                        return tint == tjso._iValue ? Boolean.True : Boolean.False;
                    }
                    case JSValueType.Double:
                    {
                        return tint == tjso._dValue ? Boolean.True : Boolean.False;
                    }
                    case JSValueType.String:
                    {
                        tstr = tjso._oValue.ToString();
                        if (Tools.ParseJsNumber(tstr, ref index, out tdouble) && index == tstr.Length)
                            return tint == tdouble ? Boolean.True : Boolean.False;
                        return false;
                    }
                    case JSValueType.Date:
                    case JSValueType.Object:
                    {
                        tjso = tjso.ToPrimitiveValue_Value_String();
                        if (tjso._valueType == JSValueType.Integer)
                            goto case JSValueType.Integer;
                        if (tjso._valueType == JSValueType.Boolean)
                            goto case JSValueType.Integer;
                        if (tjso._valueType == JSValueType.Double)
                            goto case JSValueType.Double;
                        if (tjso._valueType == JSValueType.String)
                            goto case JSValueType.String;
                        if (tjso._valueType >= JSValueType.Object) // null
                            return false;
                        throw new NotImplementedException();
                    }
                }

                return Boolean.False;
            }
            case JSValueType.Double:
            {
                tdouble = temp._dValue;
                tjso = _right.Evaluate(context);
                switch (tjso._valueType)
                {
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                    {
                        return tdouble == tjso._iValue ? Boolean.True : Boolean.False;
                    }
                    case JSValueType.Double:
                    {
                        return tdouble == tjso._dValue ? Boolean.True : Boolean.False;
                    }
                    case JSValueType.String:
                    {
                        tstr = tjso._oValue.ToString();
                        if (Tools.ParseJsNumber(tstr, ref index, out tjso._dValue) && index == tstr.Length)
                            return tdouble == tjso._dValue ? Boolean.True : Boolean.False;
                        return false;
                    }
                    case JSValueType.Date:
                    case JSValueType.Object:
                    {
                        tjso = tjso.ToPrimitiveValue_Value_String();
                        if (tjso._valueType == JSValueType.Integer)
                            goto case JSValueType.Integer;
                        if (tjso._valueType == JSValueType.Boolean)
                            goto case JSValueType.Integer;
                        if (tjso._valueType == JSValueType.Double)
                            goto case JSValueType.Double;
                        if (tjso._valueType == JSValueType.String)
                            goto case JSValueType.String;
                        if (tjso._valueType >= JSValueType.Object) // null
                            return tdouble == 0 && !NumberUtils.IsNegativeZero(tdouble);
                        throw new NotImplementedException();
                    }
                }

                return false;
            }
            case JSValueType.String:
            {
                tstr = temp._oValue.ToString();
                temp = _right.Evaluate(context);
                switch (temp._valueType)
                {
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                    {
                        if (Tools.ParseJsNumber(tstr, ref index, out tdouble) && index == tstr.Length)
                            return tdouble == temp._iValue ? Boolean.True : Boolean.False;
                        return false;
                    }
                    case JSValueType.Double:
                    {
                        if (Tools.ParseJsNumber(tstr, ref index, out tdouble) && index == tstr.Length)
                            return tdouble == temp._dValue ? Boolean.True : Boolean.False;
                        return false;
                    }
                    case JSValueType.String:
                    {
                        return string.CompareOrdinal(tstr, temp._oValue.ToString()) == 0 ? Boolean.True : Boolean.False;
                    }
                    case JSValueType.Function:
                    case JSValueType.Object:
                    {
                        temp = temp.ToPrimitiveValue_Value_String();
                        switch (temp._valueType)
                        {
                            case JSValueType.Integer:
                            case JSValueType.Boolean:
                            {
                                if (Tools.ParseJsNumber(tstr, ref index, out tdouble) && index == tstr.Length)
                                    return tdouble == temp._iValue ? Boolean.True : Boolean.False;
                                goto
                                    case JSValueType.String;
                            }
                            case JSValueType.Double:
                            {
                                if (Tools.ParseJsNumber(tstr, ref index, out tdouble) && index == tstr.Length)
                                    return tdouble == temp._dValue ? Boolean.True : Boolean.False;
                                goto case JSValueType.String;
                            }
                            case JSValueType.String:
                            {
                                return string.CompareOrdinal(tstr, temp._oValue.ToString()) == 0
                                    ? Boolean.True
                                    : Boolean.False;
                            }
                        }

                        break;
                    }
                }

                return false;
            }
            case JSValueType.Function:
            case JSValueType.Date:
            case JSValueType.Symbol:
            case JSValueType.Object:
            {
                if (_tempContainer == null)
                    _tempContainer = new JSValue { _attributes = JSValueAttributesInternal.Temporary };
                _tempContainer.Assign(temp);
                temp = _tempContainer;

                tjso = _right.Evaluate(context);
                switch (tjso._valueType)
                {
                    case JSValueType.Double:
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                    {
                        tdouble = tjso._valueType == JSValueType.Double ? tjso._dValue : tjso._iValue;
                        temp = temp.ToPrimitiveValue_Value_String();
                        switch (temp._valueType)
                        {
                            case JSValueType.Boolean:
                            case JSValueType.Integer:
                            {
                                return temp._iValue == tdouble ? Boolean.True : Boolean.False;
                            }
                            case JSValueType.Double:
                            {
                                return temp._dValue == tdouble ? Boolean.True : Boolean.False;
                            }
                            case JSValueType.String:
                            {
                                tstr = temp._oValue.ToString();
                                if (Tools.ParseJsNumber(tstr, ref index, out temp._dValue) && index == tstr.Length)
                                    return tdouble == temp._dValue ? Boolean.True : Boolean.False;
                                return false;
                            }
                        }

                        return false;
                    }
                    case JSValueType.String:
                    {
                        tstr = tjso._oValue.ToString();
                        temp = temp.ToPrimitiveValue_Value_String();
                        switch (temp._valueType)
                        {
                            case JSValueType.Double:
                            case JSValueType.Boolean:
                            case JSValueType.Integer:
                            {
                                temp._dValue = temp._valueType == JSValueType.Double ? temp._dValue : temp._iValue;
                                if (Tools.ParseJsNumber(tstr, ref index, out tdouble) && index == tstr.Length)
                                    return tdouble == temp._dValue ? Boolean.True : Boolean.False;
                                return false;
                            }
                            case JSValueType.String:
                            {
                                return temp._oValue.ToString() == tstr ? Boolean.True : Boolean.False;
                            }
                        }

                        break;
                    }
                    default:
                    {
                        return temp._oValue == tjso._oValue ? Boolean.True : Boolean.False;
                    }
                }

                break;
            }
            case JSValueType.Undefined:
            case JSValueType.NotExistsInObject:
            case JSValueType.NotExists:
            {
                temp = _right.Evaluate(context);
                switch (temp._valueType)
                {
                    case JSValueType.Object:
                    {
                        return temp._oValue == null ? Boolean.True : Boolean.False;
                    }
                    default:
                    {
                        return !temp.Defined ? Boolean.True : Boolean.False;
                    }
                }
            }
            default:
                throw new NotImplementedException();
        }

        return false;
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
        base.Optimize(ref _this, owner, message, opts, stats);
        if (message != null)
        {
            var fc = _left as Constant ?? _right as Constant;
            if (fc != null)
                switch (fc.value._valueType)
                {
                    case JSValueType.Undefined:
                        message(MessageLevel.Warning, Position, Length,
                            "To compare with undefined use '===' or '!==' instead of '==' or '!='.");
                        break;
                    case JSValueType.Object:
                        if (fc.value._oValue == null)
                            message(MessageLevel.Warning, Position, Length,
                                "To compare with null use '===' or '!==' instead of '==' or '!='.");
                        break;
                }
        }
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "(" + _left + " == " + _right + ")";
    }
}