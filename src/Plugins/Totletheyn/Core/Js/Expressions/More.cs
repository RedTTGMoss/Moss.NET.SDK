using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public class More : Expression
{
    private bool _trueMore;

    public More(Expression first, Expression second)
        : base(first, second, true)
    {
        _trueMore = GetType() == typeof(More);
    }

    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    internal static bool Check(JSValue first, JSValue second, bool lessOrEqual)
    {
        switch (first._valueType)
        {
            case JSValueType.Boolean:
            case JSValueType.Integer:
            {
                switch (second._valueType)
                {
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                    {
                        return first._iValue > second._iValue;
                    }
                    case JSValueType.Double:
                    {
                        if (double.IsNaN(second._dValue))
                            return
                                lessOrEqual; // Костыль. Для его устранения нужно делать полноценную реализацию оператора MoreOrEqual.
                        return first._iValue > second._dValue;
                    }
                    case JSValueType.String:
                    {
                        var index = 0;
                        double td = 0;
                        if (Tools.ParseJsNumber(second._oValue.ToString(), ref index, out td) &&
                            index == second._oValue.ToString().Length)
                            return first._iValue > td;
                        return lessOrEqual;
                    }
                    case JSValueType.Date:
                    case JSValueType.Object:
                    {
                        second = second.ToPrimitiveValue_Value_String();
                        if (second._valueType == JSValueType.Integer)
                            goto case JSValueType.Integer;
                        if (second._valueType == JSValueType.Boolean)
                            goto case JSValueType.Integer;
                        if (second._valueType == JSValueType.Double)
                            goto case JSValueType.Double;
                        if (second._valueType == JSValueType.String)
                            goto case JSValueType.String;
                        if (second._valueType >= JSValueType.Object) // null
                            return first._iValue > 0;
                        throw new NotImplementedException();
                    }
                    default:
                        return lessOrEqual;
                }
            }
            case JSValueType.Double:
            {
                if (double.IsNaN(first._dValue))
                    return
                        lessOrEqual; // Костыль. Для его устранения нужно делать полноценную реализацию оператора MoreOrEqual.
                switch (second._valueType)
                {
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                    {
                        return first._dValue > second._iValue;
                    }
                    case JSValueType.Double:
                    {
                        if (double.IsNaN(first._dValue) || double.IsNaN(second._dValue))
                            return
                                lessOrEqual; // Костыль. Для его устранения нужно делать полноценную реализацию оператора MoreOrEqual.
                        return first._dValue > second._dValue;
                    }
                    case JSValueType.String:
                    {
                        var index = 0;
                        double td = 0;
                        if (Tools.ParseJsNumber(second._oValue.ToString(), ref index, out td) &&
                            index == second._oValue.ToString().Length)
                            return first._dValue > td;
                        return lessOrEqual;
                    }
                    case JSValueType.Undefined:
                    case JSValueType.NotExistsInObject:
                    case JSValueType.NotExists:
                    {
                        return lessOrEqual;
                    }
                    case JSValueType.Date:
                    case JSValueType.Object:
                    {
                        second = second.ToPrimitiveValue_Value_String();
                        if (second._valueType == JSValueType.Integer)
                            goto case JSValueType.Integer;
                        if (second._valueType == JSValueType.Boolean)
                            goto case JSValueType.Integer;
                        if (second._valueType == JSValueType.Double)
                            goto case JSValueType.Double;
                        if (second._valueType == JSValueType.String)
                            goto case JSValueType.String;
                        if (second._valueType >= JSValueType.Object) // null
                            return first._dValue > 0;
                        throw new NotImplementedException();
                    }
                    default:
                        return lessOrEqual;
                }
            }
            case JSValueType.String:
            {
                var left = first._oValue.ToString();
                switch (second._valueType)
                {
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                    {
                        double d = 0;
                        var i = 0;
                        if (Tools.ParseJsNumber(left, ref i, out d) && i == left.Length)
                            return d > second._iValue;
                        return lessOrEqual;
                    }
                    case JSValueType.Double:
                    {
                        double d = 0;
                        var i = 0;
                        if (Tools.ParseJsNumber(left, ref i, out d) && i == left.Length)
                            return d > second._dValue;
                        return lessOrEqual;
                    }
                    case JSValueType.String:
                    {
                        return string.CompareOrdinal(left, second._oValue.ToString()) > 0;
                    }
                    case JSValueType.Function:
                    case JSValueType.Object:
                    {
                        second = second.ToPrimitiveValue_Value_String();
                        switch (second._valueType)
                        {
                            case JSValueType.Integer:
                            case JSValueType.Boolean:
                            {
                                var t = 0.0;
                                var i = 0;
                                if (Tools.ParseJsNumber(left, ref i, out t) && i == left.Length)
                                    return t > second._iValue;
                                goto case JSValueType.String;
                            }
                            case JSValueType.Double:
                            {
                                var t = 0.0;
                                var i = 0;
                                if (Tools.ParseJsNumber(left, ref i, out t) && i == left.Length)
                                    return t > second._dValue;
                                goto case JSValueType.String;
                            }
                            case JSValueType.String:
                            {
                                return string.CompareOrdinal(left, second._oValue.ToString()) > 0;
                            }
                            case JSValueType.Object:
                            {
                                var t = 0.0;
                                var i = 0;
                                if (Tools.ParseJsNumber(left, ref i, out t) && i == left.Length)
                                    return t > 0;
                                return lessOrEqual;
                            }
                            default: throw new NotImplementedException();
                        }
                    }
                    default:
                        return lessOrEqual;
                }
            }
            case JSValueType.Function:
            case JSValueType.Date:
            case JSValueType.Object:
            {
                first = first.ToPrimitiveValue_Value_String();
                if (first._valueType == JSValueType.Integer)
                    goto case JSValueType.Integer;
                if (first._valueType == JSValueType.Boolean)
                    goto case JSValueType.Integer;
                if (first._valueType == JSValueType.Double)
                    goto case JSValueType.Double;
                if (first._valueType == JSValueType.String)
                    goto case JSValueType.String;
                if (first._valueType >= JSValueType.Object) // null
                {
                    first._iValue = 0; // такое делать можно, поскольку тип не меняется
                    goto case JSValueType.Integer;
                }

                throw new NotImplementedException();
            }
            default:
                return lessOrEqual;
        }
    }

    public override JSValue Evaluate(Context context)
    {
        var left = _left.Evaluate(context);

        var temp = _tempContainer;
        _tempContainer = null;
        if (temp == null)
            temp = new JSValue { _attributes = JSValueAttributesInternal.Temporary };
        temp._valueType = left._valueType;
        temp._iValue = left._iValue;
        temp._oValue = left._oValue;
        temp._dValue = left._dValue;

        var right = _right.Evaluate(context);

        _tempContainer = temp;

        if (_tempContainer._valueType == JSValueType.Integer && right._valueType == JSValueType.Integer)
            return _tempContainer._iValue > right._iValue;

        if (_tempContainer._valueType == JSValueType.Double && right._valueType == JSValueType.Double)
        {
            if (double.IsNaN(temp._dValue) || double.IsNaN(right._dValue))
                return !_trueMore;
            return temp._dValue > right._dValue;
        }

        return Check(_tempContainer, right, !_trueMore);
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
        baseOptimize(ref _this, owner, message, opts, stats);
        if (_this == this)
            if (_left.ResultType == PredictedType.Number && _right.ResultType == PredictedType.Number)
                _this = new NumberMore(_left, _right);
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "(" + _left + " > " + _right + ")";
    }
}