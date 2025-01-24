using System.Runtime.InteropServices;

namespace Moss.NET.Sdk.FFI;

internal static class Functions
{
    private const string DLL = "extism";

    [DllImport(DLL, EntryPoint = "moss_gui_register_context_menu")]
    public static extern void RegisterMenu(ulong ptr);

    [DllImport(DLL, EntryPoint = "moss_em_config_get")]
    public static extern ulong GetConfig(ulong ptr);

    [DllImport(DLL, EntryPoint = "_moss_em_config_set")]
    public static extern void SetConfig(ulong ptr);

    [DllImport(DLL, EntryPoint = "moss_defaults_set_color")]
    public static extern void SetDefaultColor(ulong keyPtr, long r, long g, long b, long a);

    [DllImport(DLL, EntryPoint = "moss_defaults_set_text_color")]
    public static extern void SetDefaultTextColor(ulong keyPtr, long fr, long fg, long fb, long br, long bg, long bb);
}