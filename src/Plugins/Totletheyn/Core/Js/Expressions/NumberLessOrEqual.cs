using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class NumberLessOrEqual : Expression
{
    public NumberLessOrEqual(Expression first, Expression second)
        : base(first, second, false)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public override JSValue Evaluate(Context context)
    {
        int itemp;
        double dtemp;
        var op = _left.Evaluate(context);
        if (op._valueType == JSValueType.Integer
            || op._valueType == JSValueType.Boolean)
        {
            itemp = op._iValue;
            op = _right.Evaluate(context);
            if (op._valueType == JSValueType.Integer
                || op._valueType == JSValueType.Boolean)
                return itemp <= op._iValue;

            if (op._valueType == JSValueType.Double) return itemp <= op._dValue;

            if (_tempContainer == null)
                _tempContainer = new JSValue { _attributes = JSValueAttributesInternal.Temporary };
            _tempContainer._valueType = JSValueType.Integer;
            _tempContainer._iValue = itemp;
            return !More.Check(_tempContainer, op, true);
        }

        if (op._valueType == JSValueType.Double)
        {
            dtemp = op._dValue;
            op = _right.Evaluate(context);
            if (op._valueType == JSValueType.Integer
                || op._valueType == JSValueType.Boolean)
                return dtemp <= op._iValue;

            if (op._valueType == JSValueType.Double) return dtemp <= op._dValue;

            if (_tempContainer == null)
                _tempContainer = new JSValue { _attributes = JSValueAttributesInternal.Temporary };
            _tempContainer._valueType = JSValueType.Double;
            _tempContainer._dValue = dtemp;
            return !More.Check(_tempContainer, op, true);
        }

        if (_tempContainer == null)
            _tempContainer = new JSValue { _attributes = JSValueAttributesInternal.Temporary };
        var temp = _tempContainer;
        temp.Assign(op);
        _tempContainer = null;
        var res = !More.Check(temp, _right.Evaluate(context), true);
        _tempContainer = temp;
        return res;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "(" + _left + " <= " + _right + ")";
    }
}