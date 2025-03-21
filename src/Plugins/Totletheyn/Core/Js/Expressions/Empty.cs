using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Empty : Expression
{
    public Empty()
        : base(null, null, false)
    {
    }

    public Empty(int position)
        : base(null, null, false)
    {
        Position = position;
        Length = 0;
    }

    public static Empty Instance { get; } = new();

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => PredictedType.Undefined;

    public override JSValue Evaluate(Context context)
    {
        return null;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        return null;
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        if (expressionDepth < 2)
        {
            _this = null;
            Eliminated = true;
        }

        return false;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "";
    }
}