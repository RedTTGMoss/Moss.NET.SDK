using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Totletheyn.Core.Js.Expressions;

namespace Totletheyn.Core.Js.Core;

public enum PredictedType
{
    Unknown = 0x0,
    Ambiguous = 0x10,
    Undefined = 0x20,
    Bool = 0x30,
    Number = 0x40,
    Int = 0x41,
    Double = 0x42,
    String = 0x50,
    Object = 0x60,
    Function = 0x61,
    Class = 0x62,
    Group = 0xF0,
    Full = 0xFF
}

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public class VariableDescriptor
{
    internal readonly string name;
    internal readonly List<VariableReference> references;
    internal List<Expression> assignments;
    internal Context cacheContext;
    internal JSValue cacheRes;
    internal bool captured;
    internal int definitionScopeLevel;
    internal Expression initializer;
    internal bool isDefined;
    internal bool isReadOnly;
    internal PredictedType lastPredictedType;
    internal bool lexicalScope;
    internal CodeNode owner;
    internal int scopeBias;

    internal VariableDescriptor(string name, int definitionScopeLevel)
    {
        isDefined = true;
        this.definitionScopeLevel = definitionScopeLevel;
        this.name = name;
        references = new List<VariableReference>();
    }

    internal VariableDescriptor(VariableReference proto, int definitionDepth)
    {
        if (proto._descriptor != null)
            throw new ArgumentException("proto");

        definitionScopeLevel = definitionDepth;
        name = proto.Name;
        references = new List<VariableReference> { proto };
        proto._descriptor = this;
    }

    public bool IsDefined => isDefined;
    public CodeNode Owner => owner;
    public bool IsReadOnly => isReadOnly;
    public Expression Initializer => initializer;
    public string Name => name;
    public int ReferenceCount => references.Count;
    public bool LexicalScope => lexicalScope;
    public ReadOnlyCollection<Expression> Assignments => assignments == null ? null : assignments.AsReadOnly();

    public virtual bool IsParameter => false;

    public IEnumerable<VariableReference> References
    {
        get
        {
            for (var i = 0; i < references.Count; i++)
                yield return references[i];
        }
    }

    internal JSValue Get(Context context, bool forWrite, int scopeLevel)
    {
        context._objectSource = null;

        if (definitionScopeLevel < 0 || scopeLevel < 0)
            return context.GetVariable(name, forWrite);

        if (context == cacheContext && !forWrite)
            return cacheRes;

        return deepGet(context, forWrite, scopeLevel);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private JSValue deepGet(Context context, bool forWrite, int depth)
    {
        JSValue res = null;

        var defsl = depth - definitionScopeLevel;
        while (defsl > 0)
        {
            defsl--;
            context = context._parent;
        }

        if (context != cacheContext || cacheRes == null)
        {
            if (lexicalScope)
            {
                if (context._variables == null || !context._variables.TryGetValue(name, out res))
                    return JSValue.NotExists;
            }
            else
            {
                res = context.GetVariable(name, forWrite);
            }

            if ((!forWrite && IsDefined)
                || (res._attributes & JSValueAttributesInternal.SystemObject) == 0)
            {
                cacheContext = context;
                cacheRes = res;
            }
        }
        else
        {
            res = cacheRes;
        }

        if (forWrite && res.NeedClone)
        {
            res = context.GetVariable(name, forWrite);
            cacheRes = res;
        }

        return res;
    }

    public override string ToString()
    {
        return name;
    }
}