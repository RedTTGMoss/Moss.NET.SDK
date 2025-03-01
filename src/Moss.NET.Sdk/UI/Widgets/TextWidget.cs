using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.UI;

[SuppressMessage("Trimming",
    "IL2026:Members annotated with \'RequiresUnreferencedCodeAttribute\' require dynamic access otherwise can break functionality when trimming application code")]
public partial class TextWidget : Widget
{
    private string _font;
    private ulong _fontSize;
    private string _text;

    public TextWidget(string text, ulong fontSize, int x = 0, int y = 0, int width = 0, int height = 0)
    {
        _text = text;
        _font = Defaults.GetDefaultValue<string>("CUSTOM_FONT");
        _fontSize = fontSize;
        Id = Init();

        Bounds = new Rect(x, y, width, height);

        SetRect(Bounds);
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

    private ulong Init()
    {
        var textPtr = Pdk.Allocate(Text).Offset;
        var fontPtr = Pdk.Allocate(Font).Offset;
        var textColor = new TextColor(Foreground, Background);

        var resultPtr = MakeText(textPtr, fontPtr, FontSize, Utils.Serialize(textColor, JsonContext.Default.TextColor));
        var value = Utils.Deserialize(resultPtr, JsonContext.Default.ConfigGetD).value;

        return value.Deserialize<ulong>();
    }

    protected override void OnRender()
    {
        MossDisplayText(Id);
    }
}