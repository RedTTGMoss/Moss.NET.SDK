using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class PropertyPair : Expression
{
    public PropertyPair(Expression getter, Expression setter)
        : base(getter, setter, true)
    {
        _tempContainer._valueType = JSValueType.Property;
    }

    public Expression Getter
    {
        get => _left;
        internal set => _left = value;
    }

    public Expression Setter
    {
        get => _right;
        internal set => _right = value;
    }

    protected internal override bool ContextIndependent => false;

    public override JSValue Evaluate(Context context)
    {
        _tempContainer._oValue = new Core.PropertyPair
        (
            Getter == null ? null : (Function)Getter.Evaluate(context),
            Setter == null ? null : (Function)Setter.Evaluate(context)
        );
        return _tempContainer;
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
        throw new InvalidOperationException();
    }
}