using System;

namespace Totletheyn.Core.Js.Core.Interop;

[Prototype(typeof(JSObject), true)]
internal sealed class StaticProxy : Proxy
{
    [Hidden]
    public StaticProxy(GlobalContext context, Type type, bool indexersSupport)
        : base(context, type, indexersSupport)
    {
    }

    internal override JSObject PrototypeInstance => null;

    internal override bool IsInstancePrototype => false;
}