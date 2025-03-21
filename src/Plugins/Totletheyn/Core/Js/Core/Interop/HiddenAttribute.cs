using System;

namespace Totletheyn.Core.Js.Core.Interop;

/// <summary>
///     Член, помеченный данным аттрибутом, не будет доступен из сценария.
/// </summary>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[AttributeUsage(AttributeTargets.All, Inherited = false)]
public sealed class HiddenAttribute : Attribute
{
}