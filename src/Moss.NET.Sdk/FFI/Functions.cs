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
    public static extern ulong GetConfig(ulong ptr); //-> ConfigGet

    [DllImport(DLL, EntryPoint = "_moss_em_config_set")]
    public static extern void SetConfig(ulong ptr);

    [DllImport(DLL, EntryPoint = "moss_defaults_set_color")]
    public static extern void SetDefaultColor(ulong keyPtr, ulong colorPtr);

    [DllImport(DLL, EntryPoint = "moss_defaults_set_text_color")]
    public static extern void SetDefaultTextColor(ulong keyPtr, ulong textColorPtr);


    [DllImport(DLL, EntryPoint = "moss_defaults_get_text_color")]
    public static extern ulong GetDefaultTextColor(ulong keyPtr); // -> TextColors

    [DllImport(DLL, EntryPoint = "moss_defaults_get")]
    public static extern ulong GetDefaultValue(ulong keyPtr); //-> ConfigGet


    [DllImport(DLL, EntryPoint = "_moss_defaults_set")]
    public static extern void SetDefaultValue(ulong configSetPtr);


    [DllImport(DLL, EntryPoint = "moss_gui_invert_icon")]
    public static extern void InvertIcon(ulong keyPtr, ulong resultKeyPtr);

    [DllImport(DLL, EntryPoint = "_moss_pe_draw_rect")]
    public static extern void DrawRect(ulong extraRectPtr);

    [DllImport(DLL, EntryPoint = "moss_pe_register_screen")]
    public static extern void RegisterScreen(ulong keyPtr);

    [DllImport(DLL, EntryPoint = "_moss_pe_open_screen")]
    public static extern void OpenScreen(ulong keyPtr, ulong initial_valuesPtr); // initial_valuesPtr = dict of values, could be any object that is non primitive

    [DllImport(DLL, EntryPoint = "moss_pe_get_screen_value")]
    public static extern ulong GetScreenValue(ulong keyPtr); //-> ConfigGet

    [DllImport(DLL, EntryPoint = "_moss_pe_set_screen_value")]
    public static extern void SetScreenValue(ulong configSetPtr);

    [DllImport(DLL, EntryPoint = "moss_text_make")]
    public static extern ulong MakeText(ulong textPtr, ulong fontPtr, int font_size, ulong textColorsPtr); // -> ConfigGet<int>

    [DllImport(DLL, EntryPoint = "moss_text_set_text")]
    public static extern void SetText(int text_id, ulong textPtr);

    [DllImport(DLL, EntryPoint = "moss_text_set_font")]
    public static extern void SetFont(int text_id, ulong fontPtr, int font_size);

    [DllImport(DLL, EntryPoint = "moss_text_set_rect")]
    public static extern void SetTextRect(int text_id, ulong rectPtr);

    [DllImport(DLL, EntryPoint = "moss_text_get_rect")]
    public static extern ulong GetTextRect(int text_id, ulong rectPtr); // -> Rect

    [DllImport(DLL, EntryPoint = "moss_text_display")]
    public static extern void DisplayText(int text_id);
}