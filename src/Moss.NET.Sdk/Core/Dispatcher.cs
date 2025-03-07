using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Extism;

namespace Moss.NET.Sdk.Core;

public static class Dispatcher
{
    private static readonly LoggerInstance _logger = Log.GetLogger(nameof(Dispatcher));
    private static readonly Dictionary<ulong, Delegate> _events = new();

    public static void Register(ulong id, Delegate callback)
    {
        _events[id] = callback;
    }

    public static void Dispatch(ulong id, params object[] args)
    {
        _logger.Info("Dispatching task with id: " + id);

        if (_events.Count <= 0) return;

        if (!_events.TryGetValue(id, out var callback)) return;

        callback.DynamicInvoke(args);

        //ToDo: not removing fo objects like contextmenu
        _events.Remove(id);
    }

    [UnmanagedCallersOnly(EntryPoint = "dispatch_entry")]
    public static ulong DispatchEntry()
    {
        var input = Pdk.GetInput();
        var taskid = BitConverter.ToUInt64(input, 0);

        Dispatch(taskid);

        return 0;
    }
}