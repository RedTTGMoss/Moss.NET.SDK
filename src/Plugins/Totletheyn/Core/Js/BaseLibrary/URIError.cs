using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

[Prototype(typeof(Error))]
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class URIError : Error
{
    [DoNotEnumerate]
    public URIError()
    {
    }

    [DoNotEnumerate]
    public URIError(Arguments args)
        : base(args[0].ToString())
    {
    }

    [DoNotEnumerate]
    public URIError(string message)
        : base(message)
    {
    }
}