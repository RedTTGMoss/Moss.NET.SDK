﻿namespace Moss.NET.Sdk.Core;

public static class RunOnce
{
    private static readonly Dictionary<string, bool> actionsRun = new();
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
}