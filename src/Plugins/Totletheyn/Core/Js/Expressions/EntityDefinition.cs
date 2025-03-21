using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
internal sealed class EntityReference : VariableReference
{
    public EntityReference(EntityDefinition entityDefinition)
    {
        ScopeLevel = 1;
        _descriptor = new VariableDescriptor(entityDefinition._name, 1)
        {
            lexicalScope = !entityDefinition.Hoist,
            initializer = entityDefinition
        };
    }

    public EntityDefinition Entity => (EntityDefinition)Descriptor.initializer;

    public override string Name => Entity._name;

    public override JSValue Evaluate(Context context)
    {
        throw new InvalidOperationException();
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return Descriptor.ToString();
    }
}

public abstract class EntityDefinition : Expression
{
    internal readonly string _name;

    internal readonly VariableReference reference;

    protected EntityDefinition(string name)
    {
        _name = name;
        reference = new EntityReference(this);
    }

    [CLSCompliant(false)] protected bool Built { get; set; }

    public string Name => _name;
    public VariableReference Reference => reference;

    public abstract bool Hoist { get; }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        _codeContext = codeContext;
        return false;
    }

    public abstract override void Decompose(ref Expression self, IList<CodeNode> result);

    public override void RebuildScope(FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
    {
        reference.ScopeBias = scopeBias;
        if (reference._descriptor != null)
            if (reference._descriptor.definitionScopeLevel >= 0)
            {
                reference._descriptor.definitionScopeLevel = reference.ScopeLevel;
                reference._descriptor.scopeBias = scopeBias;
            }
    }
}