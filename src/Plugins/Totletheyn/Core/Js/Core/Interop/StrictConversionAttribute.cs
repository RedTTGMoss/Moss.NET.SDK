using System;

namespace Totletheyn.Core.Js.Core.Interop;

[AttributeUsage(
    AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Delegate,
    Inherited = false)]
public sealed class StrictConversionAttribute : Attribute
{
}