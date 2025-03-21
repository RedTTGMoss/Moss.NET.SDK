using System;
using Totletheyn.Core.Js.BaseLibrary;

namespace Totletheyn.Core.Js.Core.Interop;

public sealed class NativeError : Error
{
    public NativeError(string message) : base(message)
    {
    }

    public Exception exception { get; set; }
}