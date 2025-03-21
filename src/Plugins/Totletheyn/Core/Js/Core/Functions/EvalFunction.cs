using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core.Interop;
using Totletheyn.Core.Js.Expressions;

namespace Totletheyn.Core.Js.Core.Functions;

internal sealed class EvalFunction : Function
{
    [Hidden]
    public EvalFunction()
    {
        _length = new Number(1);
        _prototype = undefined;
        RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    [Hidden]
    public override string name
    {
        [Hidden] get => "eval";
    }

    [Field]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public override JSValue prototype
    {
        [Hidden] get => null;
        [Hidden] set { }
    }

    internal override JSValue InternalInvoke(JSValue targetObject, Expression[] arguments, Context initiator,
        bool withSpread, bool construct)
    {
        if (construct)
            ExceptionHelper.ThrowTypeError("eval can not be called as constructor");

        if (arguments == null || arguments.Length == 0)
            return NotExists;

        return base.InternalInvoke(targetObject, arguments, initiator, withSpread, construct);
    }

    protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
    {
        if (arguments == null)
            return NotExists;

        var arg = arguments[0];
        if (arg._valueType != JSValueType.String)
            return arg;

        var stack = new Stack<Context>();
        try
        {
            var ccontext = Context.CurrentContext;
            var rootContext = ccontext.RootContext;
            while (ccontext != rootContext && ccontext != null)
            {
                stack.Push(ccontext);
                ccontext = ccontext.Deactivate(false);
            }

            if (ccontext == null)
                return invokeWithContext(arguments, rootContext);
            else
                return ccontext.Eval(arguments[0].ToString());
        }
        finally
        {
            while (stack.Count != 0)
                stack.Pop().Activate(false);
        }
    }

    [ExceptionHelper.StackFrameOverride]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static JSValue invokeWithContext(Arguments arguments, Context context)
    {
        context.Activate();
        try
        {
            return context.Eval(arguments[0].ToString());
        }
        finally
        {
            context.Deactivate();
        }
    }

    [Hidden]
    public override string ToString(bool headerOnly)
    {
        var result = "function eval()";

        if (!headerOnly) result += " { [native code] }";

        return result;
    }
}