using System.Runtime.InteropServices;

namespace Moss.NET.Sdk.FFI;

public static class DispatcherEntry
{
    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_preloop")]
    public static ulong DispatcherPreLoopEntry()
    {
        Dispatcher.Dispatch(MossEvent.ScreenPreLoop);

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_postloop")]
    public static ulong DispatcherPostLoopEntry()
    {
        Dispatcher.Dispatch(MossEvent.ScreenPostLoop);

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_loop")]
    public static ulong DispatcherLoopEntry()
    {
        Dispatcher.Dispatch(MossEvent.ScreenPreLoop);

        return 0;
    }
}