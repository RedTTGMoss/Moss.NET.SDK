using System.Collections.Generic;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Expressions;

namespace Totletheyn.Core.Js.Statements;

public sealed class StoreValue : CodeNode
{
    private readonly Expression _source;

    public StoreValue(Expression source, bool forWrite)
    {
        _source = source;
        ForWrite = forWrite;
    }

    public override int Position
    {
        get => _source.Position;
        internal set => _source.Position = value;
    }

    public override int Length
    {
        get => _source.Length;
        internal set => _source.Length = value;
    }

    public bool ForWrite { get; }

    public override JSValue Evaluate(Context context)
    {
        var temp = ForWrite ? _source.EvaluateForWrite(context) : _source.Evaluate(context);

        if (context._executionMode == ExecutionMode.Suspend)
            return null;
        context.SuspendData[_source] = ForWrite ? temp : temp.CloneImpl(false);

        return null;
    }

    public override string ToString()
    {
        return _source.ToString();
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        return _source.GetChildrenImpl();
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return _source.Visit(visitor);
    }

    public override void Decompose(ref CodeNode self)
    {
    }

    public override void RebuildScope(FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
    {
        _source.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }
}