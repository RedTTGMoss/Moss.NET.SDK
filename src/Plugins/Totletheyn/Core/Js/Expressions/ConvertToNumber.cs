using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class ConvertToNumber : Expression
{
    public ConvertToNumber(Expression first)
        : base(first, null, true)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Number;

    internal override bool ResultInTempContainer => true;

    public override JSValue Evaluate(Context context)
    {
        return Tools.JSObjectToNumber(_left.Evaluate(context), _tempContainer);
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "+" + _left;
    }
}