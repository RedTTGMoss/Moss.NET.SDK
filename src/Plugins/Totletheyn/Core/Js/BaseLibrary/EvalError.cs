using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class EvalError : Error
{
    [DoNotEnumerate]
    public EvalError()
    {
    }

    [DoNotEnumerate]
    public EvalError(Arguments args)
        : base(args[0].ToString())
    {
    }

    [DoNotEnumerate]
    public EvalError(string message)
        : base(message)
    {
    }
}