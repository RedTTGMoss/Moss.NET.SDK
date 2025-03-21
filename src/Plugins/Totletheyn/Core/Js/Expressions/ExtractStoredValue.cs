﻿using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

public sealed class ExtractStoredValue : Expression
{
    public ExtractStoredValue(Expression source)
        : base(source, null, false)
    {
    }

    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose => false;

    protected internal override JSValue EvaluateForWrite(Context context)
    {
        return Evaluate(context);
    }

    public override JSValue Evaluate(Context context)
    {
        return (JSValue)context.SuspendData[_left];
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        return false;
    }

    public override string ToString()
    {
        return _left.ToString();
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
    }
}