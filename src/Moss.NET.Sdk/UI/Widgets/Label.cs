using System.Diagnostics.CodeAnalysis;
using Extism;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.UI.Widgets;

[SuppressMessage("Trimming",
    "IL2026:Members annotated with \'RequiresUnreferencedCodeAttribute\' require dynamic access otherwise can break functionality when trimming application code")]
public partial class Label : Widget
{
    private string _font = null!;
    private ulong _fontSize;
    private string _text = null!;

    public Label(string text, ulong fontSize, int x = 0, int y = 0)
    {
        Id = Init(text, fontSize);

        Bounds = new Rect(x, y, 0, 0);
        SetRect(Bounds);

        // Get Bounds to know the exact size of the widget
        Bounds = GetRect();
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;

            SetText(_text);
        }
    }

    public string Font
    {
        get => _font;
        set
        {
            _font = value;

            SetFont(value, FontSize);
        }
    }

    public ulong FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;

            SetFont(Font, value);
        }
    }

    public Rect Bounds { get; set; }

    public Color Foreground { get; set; } = Color.Black;
    public Color? Background { get; set; }

    private ulong Init(string text, ulong fontSize)
    {
        _text = text;
        _font = Defaults.GetDefaultValue<string>("CUSTOM_FONT");
        _fontSize = fontSize;

        var textPtr = Pdk.Allocate(Text).Offset;
        var fontPtr = Pdk.Allocate(Font).Offset;
        var textColor = new TextColor(Foreground, Background);

        var resultPtr = MakeText(textPtr, fontPtr, FontSize, textColor.GetPointer());
        var value = resultPtr.Get<ConfigGetD>().value;

        return value.GetUInt64();
    }

    protected override void OnRender()
    {
        MossDisplayText(Id);
    }
}