using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

public sealed class Super : Expression
{
    internal Super()
    {
    }

    public bool IsSuperConstructorCall { get; internal set; }

    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose => false;

    protected internal override bool LValueModifier => false;

    protected internal override JSValue EvaluateForWrite(Context context)
    {
        ExceptionHelper.ThrowReferenceError(Strings.InvalidLefthandSideInAssignment);
        return null;
    }

    public override JSValue Evaluate(Context context)
    {
        if (IsSuperConstructorCall)
        {
            context._objectSource = context._thisBind;
            return context._owner.__proto__;
        }

        return context._thisBind;
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        return false;
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "super";
    }
}