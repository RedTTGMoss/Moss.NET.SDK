using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class LogicalDisjunction : Expression
{
    public LogicalDisjunction(Expression first, Expression second)
        : base(first, second, false)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public override JSValue Evaluate(Context context)
    {
        var left = _left.Evaluate(context);
        if ((bool)left)
            return left;
        return _right.Evaluate(context);
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        if (message != null && expressionDepth <= 1)
            message(MessageLevel.Warning, Position, 0, "Do not use logical operator as a conditional statement");
        return base.Build(ref _this, expressionDepth, variables, codeContext | CodeContext.Conditional, message, stats,
            opts);
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "(" + _left + " || " + _right + ")";
    }
}