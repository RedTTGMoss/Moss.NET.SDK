using System.Runtime.InteropServices;
using Extism;

namespace Moss.NET.Sdk.Core;

public static class Dispatcher
{
    private static readonly LoggerInstance Logger = Log.GetLogger(nameof(Dispatcher));
    private static readonly Dictionary<ulong, Delegate> Callbacks = new();

    public static void Register(ulong id, Delegate? callback)
    {
        if (callback is null) callback = Noop;

        Callbacks[id] = callback;
    }

    private static void Noop()
    {
    }

    public static void Dispatch(ulong id, params object[] args)
    {
        if (Callbacks.Count <= 0) return;

        if (!Callbacks.TryGetValue(id, out var callback)) return;

        callback.DynamicInvoke(args);

        //ToDo: not removing fo objects like contextmenu
        Callbacks.Remove(id);
    }

    [UnmanagedCallersOnly(EntryPoint = "dispatch_entry")]
    public static ulong DispatchEntry()
    {
        var input = Pdk.GetInput();

        Logger.Info("raw task id: " + string.Join(' ', input.Select(c => Convert.ToString(c, 16))));
        var taskid = BitConverter.ToUInt64(input, 0);

        Logger.Info("Dispatching task with id: " + taskid);

        Dispatch(taskid);

        return 0;
    }
}