using System;
using System.Collections.Generic;

namespace Moss.NET.Sdk;

public static class Dispatcher
{
    private static Dictionary<MossEvent, List<Delegate>> _events = new();

    public static void Register(MossEvent ev, Delegate callback)
    {
        if (!_events.ContainsKey(ev))
        {
            _events[ev] = [];
        }

        _events[ev].Add(callback);
    }

    public static void Dispatch(MossEvent ev, params object[] args)
    {
        if (!_events.TryGetValue(ev, out var value))
        {
            return;
        }

        foreach (var callback in value)
        {
            callback.DynamicInvoke(args);
        }
    }
}