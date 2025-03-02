using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

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

    [DllImport(Functions.DLL, EntryPoint = "moss_pe_get_screen_value")]
    private static extern ulong GetScreenValue(ulong keyPtr); //-> ConfigGet

    [DllImport(Functions.DLL, EntryPoint = "_moss_pe_set_screen_value")]
    private static extern void SetScreenValue(ulong configSetPtr);

    //pe_close_screen
    [DllImport(Functions.DLL, EntryPoint = "moss_pe_close_screen")]
    internal static extern void CloseMossScreen();

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_preloop")]
    public static ulong DispatcherPreLoopEntry()
    {
        OpenedScreens.Peek().PreLoop();

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_postloop")]
    public static ulong DispatcherPostLoopEntry()
    {
        OpenedScreens.Peek().PostLoop();

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "ext_event_screen_loop")]
    public static ulong DispatcherLoopEntry()
    {
        OpenedScreens.Peek().Loop();

        return 0;
    }

    public static void Register<T>()
        where T : Screen, new()
    {
        var instance = Activator.CreateInstance<T>();
        var screen = new FFI.Screen(instance.Name, "ext_event_screen_loop", "ext_event_screen_preloop",
            "ext_event_screen_postloop");
        var screenPtr = Utils.Serialize(screen, JsonContext.Default.Screen);
        Screens.TryAdd(instance.Name, instance);

        RegisterScreen(screenPtr);
    }

    public static void OpenScreen<T>(Dictionary<string, object> values)
        where T : Screen, new()
    {
        var instance = Activator.CreateInstance<T>();

        var namePtr = Pdk.Allocate(instance.Name).Offset;
        var valuesPtr = Utils.Serialize(values, JsonContext.Default.DictionaryStringObject);

        if (!Screens.ContainsKey(instance.Name)) Register<T>();

        OpenedScreens.Push(Screens[instance.Name]);
        OpenScreen(namePtr, valuesPtr);
    }

    public static void CloseScreen()
    {
        if (OpenedScreens.TryPop(out var screen))
        {
            screen.Close();
        }

        if (OpenedScreens.Count == 0)
        {
            CloseMossScreen();
        }
    }

    public static void SetValue(string key, object value)
    {
        SetScreenValue(Utils.Serialize(new ConfigSet(key, value), JsonContext.Default.ConfigSet));
    }

    public static T GetValue<T>(string key)
    {
        var valuePtr = GetScreenValue(Pdk.Allocate(key).Offset);

        var value = Utils.Deserialize(valuePtr, JsonContext.Default.ConfigGetD).value;

        return value.GetValue<T>();
    }
}