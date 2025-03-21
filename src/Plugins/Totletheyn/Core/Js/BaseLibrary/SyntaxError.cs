using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

[Prototype(typeof(Error))]
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class SyntaxError : Error
{
    [DoNotEnumerate]
    public SyntaxError()
    {
    }

    [DoNotEnumerate]
    public SyntaxError(Arguments args)
        : base(args[0].ToString())
    {
    }

    [DoNotEnumerate]
    public SyntaxError(string message)
        : base(message)
    {
    }
}