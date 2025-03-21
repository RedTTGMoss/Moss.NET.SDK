using System;

namespace Totletheyn.Core.Js.Core.Interop;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
public sealed class NotConfigurable : Attribute
{
}