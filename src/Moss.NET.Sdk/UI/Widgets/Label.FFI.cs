using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.UI.Widgets;

public partial class Label
{
    [DllImport(Functions.DLL, EntryPoint = "moss_text_make")]
    private static extern ulong
        MakeText(ulong textPtr, ulong fontPtr, ulong font_size, ulong textColorsPtr); // -> ConfigGet<int>

    [DllImport(Functions.DLL, EntryPoint = "moss_text_set_text")]
    private static extern ulong SetText(ulong text_id, ulong textPtr); //-> Rect

    [DllImport(Functions.DLL, EntryPoint = "moss_text_set_font")]
    private static extern ulong SetFont(ulong text_id, ulong fontPtr, ulong font_size);

    [DllImport(Functions.DLL, EntryPoint = "moss_text_set_rect")]
    private static extern void SetTextRect(ulong text_id, ulong rectPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_text_get_rect")]
    private static extern ulong GetTextRect(ulong text_id); // -> Rect

    [DllImport(Functions.DLL, EntryPoint = "moss_text_display")]
    private static extern void MossDisplayText(ulong text_id);

    private void SetText(string text)
    {
        SetText(Id, text.GetPointer());
    }

    private void SetFont(string font, ulong fontSize)
    {
        SetFont(Id, font.GetPointer(), fontSize);
    }

    private void SetRect(Rect bounds)
    {
        SetTextRect(Id, bounds.GetPointer());
    }

    private Rect GetRect()
    {
        return GetTextRect(Id).Get<Rect>();
    }
}