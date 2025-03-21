using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class AssignmentOperatorCache : Expression
{
    private JSValue secondResult;

    internal AssignmentOperatorCache(Expression source)
        : base(source, null, false)
    {
    }

    public CodeNode Source => _left;

    protected internal override bool ContextIndependent => false;

    protected internal override PredictedType ResultType => _left.ResultType;

    internal override bool ResultInTempContainer => false;

    public override int Length
    {
        get => _left.Length;
        internal set => _left.Length = value;
    }

    public override int Position
    {
        get => _left.Position;
        internal set => _left.Position = value;
    }

    protected internal override JSValue EvaluateForWrite(Context context)
    {
        var res = _left.EvaluateForWrite(context);
        secondResult = Tools.GetPropertyOrValue(res, context._objectSource);
        return res;
    }

    public override JSValue Evaluate(Context context)
    {
        var res = secondResult;
        secondResult = null;
        return res;
    }

    public override string ToString()
    {
        return _left.ToString();
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        return _left.Children;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
        base.Optimize(ref _this, owner, message, opts, stats);
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        // second будем использовать как флаг isVisited
        if (_right != null)
            return false;

        _right = _left;

        _codeContext = codeContext;

        var left = _left as CodeNode;
        var res = _left.Build(ref left, expressionDepth, variables, codeContext | CodeContext.InExpression, message,
            stats, opts);
        _left = left as Expression;
        if (!res && _left is Variable)
            (_left as Variable)._throwMode = ThrowMode.ForceThrow;
        return res;
    }
}