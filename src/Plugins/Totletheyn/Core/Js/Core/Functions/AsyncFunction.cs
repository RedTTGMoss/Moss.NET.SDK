using System;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core.Interop;
using Totletheyn.Core.Js.Expressions;
using Totletheyn.Core.Js.Extensions;

namespace Totletheyn.Core.Js.Core.Functions;

[Prototype(typeof(Function), true)]
internal sealed class AsyncFunction : Function
{
    public AsyncFunction(Context context, FunctionDefinition implementation)
        : base(context, implementation)
    {
        RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    public override JSValue prototype
    {
        get => null;
        set { }
    }

    protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
    {
        if (construct)
            ExceptionHelper.ThrowTypeError("Async function cannot be invoked as a constructor");

        var body = _functionDefinition._body;
        if (body._lines.Length == 0)
        {
            notExists._valueType = JSValueType.NotExists;
            return notExists;
        }

        if (arguments == null)
            arguments = new Arguments(Context.CurrentContext);

        var internalContext = new Context(_initialContext, true, this);
        internalContext._callDepth = (Context.CurrentContext?._callDepth ?? 0) + 1;
        internalContext._definedVariables = Body._variables;

        initContext(
            targetObject,
            arguments,
            _functionDefinition._functionInfo.ContainsArguments,
            internalContext);

        initParameters(
            arguments,
            _functionDefinition._functionInfo.ContainsEval
            || _functionDefinition._functionInfo.ContainsWith
            || _functionDefinition._functionInfo.ContainsDebugger
            || _functionDefinition._functionInfo.NeedDecompose
            || (internalContext?._debugging ?? false),
            internalContext);

        var result = run(internalContext);

        result = processSuspend(internalContext, result);

        return result;
    }

    private JSValue processSuspend(Context internalContext, JSValue result)
    {
        if (internalContext._executionMode == ExecutionMode.Suspend)
        {
            var promise = internalContext._executionInfo;
            var continuator = new Сontinuator(this, internalContext);
            continuator.Build(promise);
            result = continuator.ResultPromise;
        }
        else
        {
            result = _initialContext.GlobalContext.ProxyValue(Promise.resolve(result));
        }

        return result;
    }

    [ExceptionHelper.StackFrameOverride]
    private JSValue run(Context internalContext)
    {
        internalContext.Activate();
        JSValue result = null;
        try
        {
            result = evaluateBody(internalContext);
        }
        finally
        {
            internalContext.Deactivate();
        }

        return result;
    }

    internal sealed class Сontinuator
    {
        private readonly AsyncFunction _asyncFunction;
        private readonly Context _context;

        public Сontinuator(AsyncFunction asyncFunction, Context context)
        {
            _asyncFunction = asyncFunction;
            _context = context;
        }

        public JSValue ResultPromise { get; private set; }

        public void Build(JSValue promise)
        {
            ResultPromise = subscribeOrReturnValue(promise);
        }

        private JSValue subscribeOrReturnValue(JSValue promiseOrValue)
        {
            if (promiseOrValue is null)
                return promiseOrValue;

            if (promiseOrValue.Value is Promise promise)
            {
                var result = promise.then(then, fail, false);
                return _context.GlobalContext.ProxyValue(result);
            }
            else
            {
                var thenFunc = promiseOrValue["then"];
                if (thenFunc._valueType != JSValueType.Function)
                    return promiseOrValue;

                var result = thenFunc.As<ICallable>().Call(
                    promiseOrValue,
                    new Arguments { new Func<JSValue, JSValue>(then), new Func<JSValue, JSValue>(fail) });

                return _context.GlobalContext.ProxyValue(result);
            }
        }

        private JSValue fail(JSValue arg)
        {
            return @continue(arg, ExecutionMode.ResumeThrow);
        }

        private JSValue then(JSValue arg)
        {
            return @continue(arg, ExecutionMode.Resume);
        }

        private JSValue @continue(JSValue arg, ExecutionMode mode)
        {
            _context._executionInfo = arg;
            _context._executionMode = mode;

            JSValue result = null;
            result = _asyncFunction.run(_context);

            return subscribeOrReturnValue(result);
        }
    }
}