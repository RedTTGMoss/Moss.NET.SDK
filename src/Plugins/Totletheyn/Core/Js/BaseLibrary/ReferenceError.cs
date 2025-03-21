using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

[Prototype(typeof(Error))]
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class ReferenceError : Error
{
    [DoNotEnumerate]
    public ReferenceError(Arguments args)
        : base(args[0].ToString())
    {
    }

    [DoNotEnumerate]
    public ReferenceError()
    {
    }

    [DoNotEnumerate]
    public ReferenceError(string message)
        : base(message)
    {
    }
}