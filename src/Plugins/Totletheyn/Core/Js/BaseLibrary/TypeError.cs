using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

[Prototype(typeof(Error))]
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class TypeError : Error
{
    [DoNotEnumerate]
    public TypeError(Arguments args)
        : base(args[0].ToString())
    {
    }

    [DoNotEnumerate]
    public TypeError()
    {
    }

    [DoNotEnumerate]
    public TypeError(string message)
        : base(message)
    {
    }
}