﻿using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class NotEqual : Equal
{
    public NotEqual(Expression first, Expression second)
        : base(first, second)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Bool;

    public override JSValue Evaluate(Context context)
    {
        return base.Evaluate(context)._iValue == 0;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "(" + _left + " != " + _right + ")";
    }
}