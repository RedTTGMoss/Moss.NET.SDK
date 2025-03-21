using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;
using Totletheyn.Core.Js.Statements;

namespace Totletheyn.Core.Js.Expressions;

public enum CallMode
{
    Regular = 0,
    Construct,
    Super
}

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Call : Expression
{
    internal bool _allowTCO;
    internal CallMode _callMode;
    internal bool _withSpread;

    public Call(Expression first, Expression[] arguments)
        : this(first, arguments, false)
    {
    }

    public Call(Expression first, Expression[] arguments, bool optionalChaining)
        : base(first, null, false)
    {
        Arguments = arguments;
        OptionalChaining = optionalChaining;
    }

    public CallMode CallMode => _callMode;
    protected internal override bool ContextIndependent => false;
    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType
    {
        get
        {
            if (_left is VariableReference)
            {
                var desc = (_left as VariableReference)._descriptor;
                var fe = desc.initializer as FunctionDefinition;
                if (fe != null)
                    return fe._functionInfo.ResultType; // для рекурсивных функций будет Unknown
            }

            return PredictedType.Unknown;
        }
    }

    public Expression[] Arguments { get; }

    public bool AllowTCO => _allowTCO && _callMode == 0;

    public bool OptionalChaining { get; }

    protected internal override bool NeedDecompose
    {
        get
        {
            if (_left.NeedDecompose)
                return true;

            for (var i = 0; i < Arguments.Length; i++)
                if (Arguments[i].NeedDecompose)
                    return true;

            return false;
        }
    }

    public override JSValue Evaluate(Context context)
    {
        if (context._callDepth >= 700)
            ExceptionHelper.Throw(new RangeError("Stack overflow."), this, context);

        var function = _left.Evaluate(context);

        var targetObject = context._objectSource;

        Function func = null;
        if (function._valueType == JSValueType.Function)
            func = function._oValue as Function;

        if (func == null)
            return callCallable(context, targetObject, function);

        if (_allowTCO
            && _callMode == 0
            && context._owner != null
            && func == context._owner._oValue
            && func._functionDefinition._kind is not FunctionKind.Generator
                and not FunctionKind.MethodGenerator
                and not FunctionKind.AnonymousGenerator)
        {
            tailCall(context, func);
            context._objectSource = targetObject;
            return JSValue.undefined;
        }

        context._objectSource = null;

        if (_callMode == CallMode.Construct)
            targetObject = null;

        JSValue result;
        if ((function._attributes & JSValueAttributesInternal.Eval) != 0)
            result = callEval(context);
        else
            result = func.InternalInvoke(targetObject, Arguments, context, _withSpread, _callMode != 0);

        return result;
    }

    private void throwNaF(Context context)
    {
        for (var i = 0; i < Arguments.Length; i++)
        {
            context._objectSource = null;
            Arguments[i].Evaluate(context);
        }

        context._objectSource = null;

        // Аргументы должны быть вычислены даже если функция не существует.
        ExceptionHelper.ThrowTypeError(_left + " is not a function", this, context);
    }

    private JSValue callCallable(Context context, JSValue targetObject, JSValue function)
    {
        var callable = function._oValue as ICallable;
        if (callable == null)
            callable = function.Value as ICallable;

        if (callable == null)
        {
            var typeProxy = function.Value as Proxy;
            if (typeProxy != null)
                callable = typeProxy.PrototypeInstance as ICallable;
        }

        if (callable == null)
        {
            if (OptionalChaining)
                return JSValue.undefined;

            throwNaF(context);

            return null;
        }

        switch (_callMode)
        {
            case CallMode.Construct:
                return callable.Construct(Tools.CreateArguments(Arguments, context));

            case CallMode.Super:
                return callable.Construct(targetObject, Tools.CreateArguments(Arguments, context));

            default:
                return callable.Call(targetObject, Tools.CreateArguments(Arguments, context));
        }
    }

    private JSValue callEval(Context context)
    {
        if (_callMode != CallMode.Regular)
            ExceptionHelper.ThrowTypeError("function eval(...) cannot be called as a constructor");

        if (Arguments == null || Arguments.Length == 0)
            return JSValue.NotExists;

        var evalCode = Arguments[0].Evaluate(context);

        for (var i = 1; i < Arguments.Length; i++)
        {
            context._objectSource = null;
            Arguments[i].Evaluate(context);
        }

        if (evalCode._valueType != JSValueType.String)
            return evalCode;

        return context.Eval(evalCode.ToString());
    }

    private void tailCall(Context context, Function func)
    {
        context._executionMode = ExecutionMode.TailRecursion;

        var arguments = new Arguments(context);

        for (var i = 0; i < Arguments.Length; i++)
            arguments.Add(Tools.EvalExpressionSafe(context, Arguments[i]));
        context._objectSource = null;

        arguments._callee = func;
        context._executionInfo = arguments;
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        if (stats != null)
            stats.UseCall = true;

        _codeContext = codeContext;

        var super = _left as Super;

        if (super != null)
        {
            super.IsSuperConstructorCall = true;
            _callMode = CallMode.Super;
        }

        for (var i = 0; i < Arguments.Length; i++)
            Parser.Build(ref Arguments[i], expressionDepth + 1, variables, codeContext | CodeContext.InExpression,
                message, stats, opts);

        base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
        if (_left is Variable)
        {
            var name = _left.ToString();
            if (name == "eval" && stats != null)
            {
                stats.ContainsEval = true;
                foreach (var variable in variables) variable.Value.captured = true;
            }

            VariableDescriptor f = null;
            if (variables.TryGetValue(name, out f))
            {
                var func = f.initializer as FunctionDefinition;
                if (func != null)
                    for (var i = 0; i < func._parameters.Length; i++)
                    {
                        if (i >= Arguments.Length)
                            break;
                        if (func._parameters[i].lastPredictedType == PredictedType.Unknown)
                            func._parameters[i].lastPredictedType = Arguments[i].ResultType;
                        else if (Tools.CompareWithMask(func._parameters[i].lastPredictedType, Arguments[i].ResultType,
                                     PredictedType.Group) != 0)
                            func._parameters[i].lastPredictedType = PredictedType.Ambiguous;
                    }
            }
        }

        return false;
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
        base.Optimize(ref _this, owner, message, opts, stats);
        for (var i = Arguments.Length; i-- > 0;)
        {
            var cn = Arguments[i] as CodeNode;
            cn.Optimize(ref cn, owner, message, opts, stats);
            Arguments[i] = cn as Expression;
        }
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        var result = new CodeNode[Arguments.Length + 1];
        result[0] = _left;
        Arguments.CopyTo(result, 1);
        return result;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
        _left.Decompose(ref _left, result);

        var lastDecomposeIndex = -1;
        for (var i = 0; i < Arguments.Length; i++)
        {
            Arguments[i].Decompose(ref Arguments[i], result);
            if (Arguments[i].NeedDecompose) lastDecomposeIndex = i;
        }

        for (var i = 0; i < lastDecomposeIndex; i++)
            if (!(Arguments[i] is ExtractStoredValue))
            {
                result.Add(new StoreValue(Arguments[i], false));
                Arguments[i] = new ExtractStoredValue(Arguments[i]);
            }
    }

    public override void RebuildScope(FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
    {
        base.RebuildScope(functionInfo, transferedVariables, scopeBias);

        for (var i = 0; i < Arguments.Length; i++)
            Arguments[i].RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
        var res = _left + "(";
        for (var i = 0; i < Arguments.Length; i++)
        {
            res += Arguments[i];
            if (i + 1 < Arguments.Length)
                res += ", ";
        }

        res += ")";

        if (_callMode == CallMode.Construct)
            return "new " + res;
        return res;
    }
}