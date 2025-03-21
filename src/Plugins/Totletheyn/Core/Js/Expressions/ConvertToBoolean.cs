using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class ConvertToBoolean : Expression
{
    public ConvertToBoolean(Expression first)
        : base(first, null, false)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public override JSValue Evaluate(Context context)
    {
        return (bool)_left.Evaluate(context);
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "!!" + _left;
    }
}