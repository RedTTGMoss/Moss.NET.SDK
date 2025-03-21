using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

[Prototype(typeof(Error))]
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class RangeError : Error
{
    [DoNotEnumerate]
    public RangeError()
    {
    }

    [DoNotEnumerate]
    public RangeError(Arguments args)
        : base(args[0].ToString())
    {
    }

    [DoNotEnumerate]
    public RangeError(string message)
        : base(message)
    {
    }
}