using System.Runtime.InteropServices;

namespace Moss.NET.Sdk.FFI;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
internal static class Functions
{
    private const string DLL = "extism";

    [DllImport(DLL, EntryPoint = "moss_gui_register_context_menu")]
    public static extern void RegisterContextMenu(ulong ptr);


    [DllImport(DLL, EntryPoint = "moss_gui_open_context_menu")]
    public static extern void OpenContextMenu(ulong keyPtr, int x, int y);


    [DllImport(DLL, EntryPoint = "moss_em_config_get")]
    public static extern ulong GetConfig(ulong ptr);

    [DllImport(DLL, EntryPoint = "_moss_em_config_set")]
    public static extern void SetConfig(ulong ptr);

    [DllImport(DLL, EntryPoint = "moss_defaults_set_color")]
    public static extern void SetDefaultColor(ulong keyPtr, ulong colorPtr);

    [DllImport(DLL, EntryPoint = "moss_defaults_set_text_color")]
    public static extern void SetDefaultTextColor(ulong keyPtr, ulong textColorPtr);


    [DllImport(DLL, EntryPoint = "moss_defaults_get_text_color")]
    public static extern void GetDefaultTextColor(ulong keyPtr);

    [DllImport(DLL, EntryPoint = "moss_defaults_get")]
    public static extern void GetDefaultValue(ulong keyPtr);


    [DllImport(DLL, EntryPoint = "_moss_defaults_set")]
    public static extern void SetDefaultValue(ulong configSetPtr);


    [DllImport(DLL, EntryPoint = "moss_gui_invert_icon")]
    public static extern void InvertIcon(ulong keyPtr, ulong resultKeyPtr);
}