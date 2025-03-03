using System.Runtime.CompilerServices;

namespace Moss.NET.Sdk.Core;

using System;
using System.Collections.Generic;

public static class RunOnce
{
    private static readonly Dictionary<string, bool> actionsRun = new Dictionary<string, bool>();
    private static readonly object _lockObj = new();

    public static void Execute(string actionKey, Action action)
    {
        lock (_lockObj)
        {
            if (actionsRun.ContainsKey(actionKey) && actionsRun[actionKey])
                return;

            action();
            actionsRun[actionKey] = true;
        }
    }

    public static unsafe void Execute(Action action)
    {
        Execute(new IntPtr(Unsafe.AsPointer(ref action)).ToString(), action);
    }
}