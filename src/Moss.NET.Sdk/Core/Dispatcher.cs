using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Extism;

namespace Moss.NET.Sdk.Core;

public static class Dispatcher
{
    private static readonly LoggerInstance _logger = Log.GetLogger(nameof(Dispatcher));
    private static readonly Dictionary<ulong, Delegate> _events = new();

    public static void Register(ulong id, Delegate? callback)
    {
        if (callback is null) callback = Noop;

        _events[id] = callback;
    }

    private static void Noop()
    {
    }

    public static void Dispatch(ulong id, params object[] args)
    {
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

        _logger.Info("raw task id: " + string.Join(' ', input.Select(c => Convert.ToString(c, 16))));
        var taskid = BitConverter.ToUInt64(input, 0);

        _logger.Info("Dispatching task with id: " + taskid);

        Dispatch(taskid);

        return 0;
    }
}