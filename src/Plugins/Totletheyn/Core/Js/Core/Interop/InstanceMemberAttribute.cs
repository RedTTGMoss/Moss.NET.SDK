using System;

namespace Totletheyn.Core.Js.Core.Interop;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
public sealed class InstanceMemberAttribute : Attribute
{
}