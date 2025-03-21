using System;

namespace Totletheyn.Core.Js.Core.Interop;

/// <summary>
///     Указывает, что помеченный член следует пропустить при перечислении в конструкции for-in
/// </summary>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[AttributeUsage(
    AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property |
    AttributeTargets.Constructor, Inherited = false)]
public sealed class DoNotEnumerateAttribute : Attribute
{
}