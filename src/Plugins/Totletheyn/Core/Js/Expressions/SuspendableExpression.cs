﻿using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

public sealed class SuspendableExpression : Expression
{
    private readonly Expression _original;
    private readonly CodeNode[] _parts;

    internal SuspendableExpression(Expression prototype, CodeNode[] parts)
    {
        _original = prototype;
        _parts = parts;
    }

    protected internal override bool ContextIndependent => false;

    public override JSValue Evaluate(Context context)
    {
        var i = 0;

        if (context._executionMode >= ExecutionMode.Resume) i = (int)context.SuspendData[this];

        for (; i < _parts.Length; i++)
        {
            _parts[i].Evaluate(context);
            if (context._executionMode == ExecutionMode.Suspend)
            {
                context.SuspendData[this] = i;
                return null;
            }
        }

        var result = _original.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
            context.SuspendData[this] = i;
            return null;
        }

        return result;
    }

    public override string ToString()
    {
        return _original.ToString();
    }
}