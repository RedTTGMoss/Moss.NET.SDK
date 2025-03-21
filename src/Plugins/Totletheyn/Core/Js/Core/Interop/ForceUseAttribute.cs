﻿using System;

namespace Totletheyn.Core.Js.Core.Interop;

/// <summary>
///     Указывает на необходимость учитывать член при создании представителя в среде выполнения сценария
///     вне зависимости от модификатора доступа
/// </summary>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[AttributeUsage(AttributeTargets.All
#if (PORTABLE || NETCORE)
    & ~AttributeTargets.Constructor
#endif
)]
#if !WRC
public
#endif
    sealed class ForceUseAttribute : Attribute
{
}