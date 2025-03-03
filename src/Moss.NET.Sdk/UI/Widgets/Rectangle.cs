using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.UI.Widgets;

public class Rectangle(Color color, int x, int y, int width, int height)
    : Widget
{
    public Rect Bounds { get; set; } = new(x, y, width, height);
    public Color Color { get; set; } = color;

    [DllImport(Functions.DLL, EntryPoint = "_moss_pe_draw_rect")]
    private static extern void DrawRect(ulong extraRectPtr);

    protected override void OnRender()
    {
        var extraRect = new PygameExtraRect(Color, Bounds, Bounds.width);
        var extraRectPtr = Utils.Serialize(extraRect, JsonContext.Default.PygameExtraRect);

        DrawRect(extraRectPtr);
    }
}