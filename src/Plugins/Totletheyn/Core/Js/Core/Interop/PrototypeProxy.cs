using System;

namespace Totletheyn.Core.Js.Core.Interop;

[Prototype(typeof(JSObject), true)]
internal sealed class PrototypeProxy : Proxy
{
    public PrototypeProxy(GlobalContext context, Type type, bool indexersSupport)
        : base(context, type, indexersSupport)
    {
    }

    internal override bool IsInstancePrototype => true;
}