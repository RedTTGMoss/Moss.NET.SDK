using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.UI;

public static class ScreenManager
{
    private static readonly Dictionary<string, Screen> Screens = new();
    private static Stack<Screen> OpenedScreens { get; } = new();

    [DllImport(Functions.DLL, EntryPoint = "moss_pe_register_screen")]
    private static extern void RegisterScreen(ulong keyPtr);

    [DllImport(Functions.DLL, EntryPoint = "_moss_pe_open_screen")]
    private static extern ulong
        OpenScreen(ulong keyPtr,
            ulong initial_valuesPtr); // initial_valuesPtr = dict of values, could be any object that is non primitive

    //pe_close_screen
    [DllImport(Functions.DLL, EntryPoint = "moss_pe_close_screen")]
    internal static extern void CloseMossScreen();

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_preloop")]
    public static ulong PreLoopEntry()
    {
        if (OpenedScreens.Count == 0) return 0;

        OpenedScreens.Peek().PreLoop();

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_postloop")]
    public static ulong PostLoopEntry()
    {
        if (OpenedScreens.Count == 0) return 0;

        OpenedScreens.Peek().PostLoop();

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_loop")]
    public static ulong LoopEntry()
    {
        if (OpenedScreens.Count == 0) return 0;

        OpenedScreens.Peek().Loop();

        return 0;
    }

    public static void Register<T>()
        where T : Screen, new()
    {
        var instance = Activator.CreateInstance<T>();
        var screen = new FFI.Dto.Screen(instance.Name, "ext_event_screen_loop", "ext_event_screen_preloop",
            "ext_event_screen_postloop");

        Screens.TryAdd(instance.Name, instance);
        RegisterScreen(screen.GetPointer());
    }

    public static void Open<T>(Dictionary<string, object> values)
        where T : Screen, new()
    {
        var instance = Activator.CreateInstance<T>();

        if (!Screens.ContainsKey(instance.Name)) Register<T>();

        OpenedScreens.Push(Screens[instance.Name]);
        OpenScreen(instance.Name.GetPointer(), values.GetPointer());
    }

    public static void Close()
    {
        if (OpenedScreens.TryPop(out var screen)) screen.Close();

        if (OpenedScreens.Count == 0) CloseMossScreen();
    }
}