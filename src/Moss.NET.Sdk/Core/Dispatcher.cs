using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Moss.NET.Sdk.Core;

public static class Dispatcher
{
    private static readonly LoggerInstance _logger = Log.GetLogger(nameof(Dispatcher));
    private static readonly Dictionary<MossEvent, List<Delegate>> _events = new();

    public static void Register(MossEvent ev, Delegate callback)
    {
        if (!_events.ContainsKey(ev)) _events[ev] = [];

        _events[ev].Add(callback);
    }

    public static void Dispatch(MossEvent ev, params object[] args)
    {
        if (!_events.TryGetValue(ev, out var value)) return;

        foreach (var callback in value) callback.DynamicInvoke(args);
    }

    [UnmanagedCallersOnly(EntryPoint = "dispatch_entry")]
    public static ulong DispatchEntry()
    {
        //todo: implement event dispatching
        _logger.Info("dispatching event");

        return 0;
    }
}