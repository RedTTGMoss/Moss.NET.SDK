using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Extensions;

public static class ContextExtensions
{
    public static void Add(this Context context, string key, object value)
    {
        context.DefineVariable(key).Assign(context.GlobalContext.ProxyValue(value));
    }

    public static void Add(this Context context, string key, JSValue value)
    {
        context.DefineVariable(key).Assign(value);
    }
}