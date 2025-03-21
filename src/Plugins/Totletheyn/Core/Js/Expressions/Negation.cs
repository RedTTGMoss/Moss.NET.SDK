using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Negation : Expression
{
    public Negation(Expression first)
        : base(first, null, true)
    {
    }

    protected internal override PredictedType ResultType =>
        PredictedType.Number; // -int.MinValue == int.MinValue, но должно быть -int.MinValue == -(double)int.MinValue;

    internal override bool ResultInTempContainer => true;

    public override JSValue Evaluate(Context context)
    {
        var val = _left.Evaluate(context);
        if (val._valueType == JSValueType.Integer
            || val.ValueType == JSValueType.Boolean)
        {
            if (val._iValue == 0)
            {
                _tempContainer._valueType = JSValueType.Double;
                _tempContainer._dValue = -0.0;
            }
            else
            {
                if (val._iValue == int.MinValue)
                {
                    _tempContainer._valueType = JSValueType.Double;
                    _tempContainer._dValue = val._iValue;
                }
                else
                {
                    _tempContainer._valueType = JSValueType.Integer;
                    _tempContainer._iValue = -val._iValue;
                }
            }
        }
        else
        {
            _tempContainer._dValue = -Tools.JSObjectToDouble(val);
            _tempContainer._valueType = JSValueType.Double;
        }

        return _tempContainer;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "-" + _left;
    }
}