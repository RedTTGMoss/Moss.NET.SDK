using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.UI;

public partial class TextWidget
{
    [DllImport(Functions.DLL, EntryPoint = "moss_text_make")]
    private static extern ulong MakeText(ulong textPtr, ulong fontPtr, ulong font_size, ulong textColorsPtr); // -> ConfigGet<int>

    [DllImport(Functions.DLL, EntryPoint = "moss_text_set_text")]
    private static extern ulong SetText(ulong text_id, ulong textPtr); //-> Rect

    [DllImport(Functions.DLL, EntryPoint = "moss_text_set_font")]
    private static extern ulong SetFont(ulong text_id, ulong fontPtr, ulong font_size);

    [DllImport(Functions.DLL, EntryPoint = "moss_text_set_rect")]
    private static extern void SetTextRect(ulong text_id, ulong rectPtr);

    //[DllImport(Functions.DLL, EntryPoint = "moss_text_get_rect")]
    private static extern ulong GetTextRect(ulong text_id, ulong rectPtr); // -> Rect

    [DllImport(Functions.DLL, EntryPoint = "moss_text_display")]
    private static extern void MossDisplayText(ulong text_id);

    private void SetText(string text)
    {
        SetText(Id, Pdk.Allocate(text).Offset);
    }

    private void SetFont(string font, ulong fontSize)
    {
        SetFont(Id, Pdk.Allocate(font).Offset, fontSize);
    }

    private void SetRect(Rect bounds)
    {
        SetTextRect(Id, Utils.Serialize(bounds, JsonContext.Default.Rect));
    }
}