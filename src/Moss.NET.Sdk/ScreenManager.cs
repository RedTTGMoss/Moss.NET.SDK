using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public static class ScreenManager
{
    [DllImport(Functions.DLL, EntryPoint = "moss_pe_register_screen")]
    private static extern void RegisterScreen(ulong keyPtr);

    [DllImport(Functions.DLL, EntryPoint = "_moss_pe_open_screen")]
    private static extern void OpenScreen(ulong keyPtr, ulong initial_valuesPtr); // initial_valuesPtr = dict of values, could be any object that is non primitive

    [DllImport(Functions.DLL, EntryPoint = "moss_pe_get_screen_value")]
    private static extern ulong GetScreenValue(ulong keyPtr); //-> ConfigGet

    [DllImport(Functions.DLL, EntryPoint = "_moss_pe_set_screen_value")]
    private static extern void SetScreenValue(ulong configSetPtr);

    public static void Register<T>()
        where T : IScreen
    {
        var screen = new Screen(T.Name, "ext_event_screen_loop", "ext_event_screen_pre_loop", "ext_event_screen_post_loop");
        var screenPtr = Utils.Serialize(screen, JsonContext.Default.Screen);
        Dispatcher.Register(MossEvent.ScreenLoop, T.Loop);

        RegisterScreen(screenPtr);
    }

    public static void OpenScreen(string name, Dictionary<string, object> values)
    {
        var namePtr = Pdk.Allocate(name).Offset;
        var valuesPtr = Utils.Serialize(values, JsonContext.Default.DictionaryStringObject);


        OpenScreen(namePtr, valuesPtr);
    }
}