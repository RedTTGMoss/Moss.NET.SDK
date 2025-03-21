using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

public sealed class NewTarget : Expression
{
    private readonly int _lexicalScopeDepth;

    public NewTarget(int lexicalScopeDepth)
    {
        _lexicalScopeDepth = lexicalScopeDepth;
    }

    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose => false;

    protected internal override bool LValueModifier => false;

    public override JSValue Evaluate(Context context)
    {
        if (context._thisBind != null
            && (context._thisBind._attributes & JSValueAttributesInternal.ConstructingObject) != 0
            && context._thisBind is ConstructableValue constrValue)
            return constrValue.NewTarget;

        return JSValue.undefined;
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
        return "new.target";
    }
}