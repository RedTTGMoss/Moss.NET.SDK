using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Expressions;

namespace Totletheyn.Core.Js.Core;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public abstract class VariableReference : Expression
{
    internal VariableDescriptor _descriptor;

    private int _scopeBias;

    internal int _scopeLevel;
    public VariableDescriptor Descriptor => _descriptor;

    public int ScopeLevel
    {
        get => _scopeLevel;
        internal set => _scopeLevel = value + _scopeBias;
    }

    public bool IsCacheEnabled => _scopeLevel >= 0;

    public int ScopeBias
    {
        get => _scopeBias;
        internal set
        {
            var sign = Math.Sign(_scopeLevel);
            _scopeLevel -= _scopeBias * sign;
            _scopeLevel += value * sign;
            _scopeBias = value;
        }
    }

    public abstract string Name { get; }

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => _descriptor.lastPredictedType;

    protected internal override CodeNode[] GetChildrenImpl()
    {
        return null;
    }

    public override void RebuildScope(FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
    {
        ScopeBias = scopeBias;

        VariableDescriptor desc = null;
        if (transferedVariables != null && transferedVariables.TryGetValue(Name, out desc))
        {
            _descriptor?.references.Remove(this);
            desc.references.Add(this);
            _descriptor = desc;
        }
    }
}