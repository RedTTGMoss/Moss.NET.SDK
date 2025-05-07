using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.Scheduler;
using TaskScheduler = Moss.NET.Sdk.Scheduler.TaskScheduler;

namespace Moss.NET.Sdk.FFI;

internal static class MossEntry
{
    [UnmanagedCallersOnly(EntryPoint = "moss_extension_unregister")]
    public static ulong Unregister()
    {
        TaskScheduler.SaveTasks();
        MossExtension.Instance!.Unregister();

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_loop")]
    public static ulong ExtensionLoop()
    {
        var state = Pdk.GetInputJson(JsonContext.Default.MossState)!;

        TaskScheduler.RunJobs();

        MossExtension.Instance?.ExtensionLoop(state);

        return 0;
    }
}