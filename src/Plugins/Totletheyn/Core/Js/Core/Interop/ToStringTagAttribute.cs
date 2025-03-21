using System;

namespace Totletheyn.Core.Js.Core.Interop;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
public sealed class ToStringTagAttribute : Attribute
{
    public ToStringTagAttribute(string tag)
    {
        Tag = tag;
    }

    public string Tag { get; }
}