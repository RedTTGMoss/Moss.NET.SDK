using System;

namespace Totletheyn.Core.Js.Core.Interop;

/// <summary>
///     Член, помеченный данным аттрибутом, не будет удаляться оператором "delete".
/// </summary>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false)]
public sealed class DoNotDeleteAttribute : Attribute
{
}