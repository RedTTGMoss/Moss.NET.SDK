using System.Runtime.InteropServices;

namespace Moss.NET.Sdk;

internal static class FFI
{
    [DllImport("extism", EntryPoint = "moss_gui_register_context_menu")]
    public static extern void RegisterMenu(ulong ptr);

    [DllImport("extism", EntryPoint = "moss_em_config_get")]
    public static extern ulong GetConfig(ulong ptr);

    [DllImport("extism", EntryPoint = "_moss_em_config_set")]
    public static extern void SetConfig(ulong ptr);

    [DllImport("extism", EntryPoint = "moss_defaults_set_color")]
    public static extern void SetDefaultColor(ulong keyPtr, ulong r, ulong g, ulong b, ulong a);
}