﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public class DrawingContext
{
    [DllImport(Functions.DLL, EntryPoint = "_moss_pe_draw_rect")]
    private static extern void DrawRect(ulong extraRectPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_text_make")]
    private static extern ulong MakeText(ulong textPtr, ulong fontPtr, long font_size, ulong textColorsPtr); // -> ConfigGet<int>

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

    public static void DrawRect(Color color, int x, int y, int width, int height)
    {
        var extraRect = new PygameExtraRect(color, new Rect(x, y, width, height), width);
        var extraRectPtr = Utils.Serialize(extraRect, JsonContext.Default.PygameExtraRect);

        DrawRect(extraRectPtr);
    }

    public static ulong MakeText(string text, long fontSize, Color? foreground = null)
    {
        var textPtr = Pdk.Allocate(text).Offset;
        var fontPtr = Pdk.Allocate(Defaults.GetDefaultValue<string>("CUSTOM_FONT")).Offset;
        var textColor = new TextColor(foreground ?? Color.Black);

        var resultPtr = MakeText(textPtr, fontPtr, fontSize, Utils.Serialize(textColor, JsonContext.Default.TextColor));
        var value = Utils.Deserialize(resultPtr, JsonContext.Default.ConfigGetD).value;

        return value.Deserialize<ulong>();
    }

    public static void DisplayText(ulong textID, Rect rect)
    {
        SetTextRect(textID, Utils.Serialize(rect, JsonContext.Default.Rect));
        MossDisplayText(textID);
    }

    public static void SetText(ulong id, string text)
    {
        SetText(id, Pdk.Allocate(text).Offset);
    }

    public static void SetFont(ulong id, string font, ulong fontSize)
    {
        SetFont(id, Pdk.Allocate(font).Offset, fontSize);
    }
}