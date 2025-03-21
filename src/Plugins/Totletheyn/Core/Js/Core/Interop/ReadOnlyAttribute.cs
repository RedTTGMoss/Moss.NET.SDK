using System;

namespace Totletheyn.Core.Js.Core.Interop;

/// <summary>
///     Значение поля, помеченного данным аттрибутом, будет неизменяемо для скрипта.
/// </summary>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field, Inherited = false)]
public sealed class ReadOnlyAttribute : Attribute
{
}