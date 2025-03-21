using System;

namespace Totletheyn.Core.Js.Core.Interop;

/// <summary>
///     Служит для передачи в среду выполнения скрипта информации о количестве ожидаемых параметров метода.
/// </summary>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
internal sealed class ArgumentsCountAttribute : Attribute
{
    public ArgumentsCountAttribute(int count)
    {
        Count = count;
    }

    public int Count { get; private set; }
}