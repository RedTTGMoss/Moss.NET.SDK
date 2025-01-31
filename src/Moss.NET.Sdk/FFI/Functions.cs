using System.Runtime.InteropServices;

namespace Moss.NET.Sdk.FFI;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
internal static class Functions
{
    internal const string DLL = "extism";

    [DllImport(DLL, EntryPoint = "moss_gui_register_context_menu")]
    public static extern void RegisterContextMenu(ulong ptr);


    [DllImport(DLL, EntryPoint = "moss_gui_open_context_menu")]
    public static extern void OpenContextMenu(ulong keyPtr, long x, long y);


    [DllImport(DLL, EntryPoint = "moss_gui_invert_icon")]
    public static extern void InvertIcon(ulong keyPtr, ulong resultKeyPtr);

}