using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class BitwiseConjunction : Expression
{
    public BitwiseConjunction(Expression first, Expression second)
        : base(first, second, true)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Int;

    internal override bool ResultInTempContainer => true;

    public override JSValue Evaluate(Context context)
    {
        _tempContainer._iValue = Tools.JSObjectToInt32(_left.Evaluate(context)) &
                                 Tools.JSObjectToInt32(_right.Evaluate(context));
        _tempContainer._valueType = JSValueType.Integer;
        return _tempContainer;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "(" + _left + " & " + _right + ")";
    }
}