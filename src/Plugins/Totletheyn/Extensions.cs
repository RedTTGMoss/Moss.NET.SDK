using System;
using Totletheyn.Core.Js.Core;

namespace Totletheyn;

public static class Extensions
{
    public static void DefineFunction(this Context context, string name, Delegate function)
    {
        context.DefineVariable(name).Assign(JSValue.Marshal(function));
    }
}