using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public abstract class Theme
{
    protected abstract Color Background { get; }
    protected abstract Color Selected { get; }
    protected abstract Color LineGray { get; }
    protected abstract Color LineGrayLight { get; }
    protected abstract Color DocumentGray { get; }
    protected abstract Color DocumentBackground { get; }
    protected abstract Color ButtonActiveColor { get; }
    protected abstract Color ButtonActiveColorInverted { get; }
    protected abstract Color ButtonDisabledColor { get; }
    protected abstract Color ButtonDisabledLightColor { get; }
    protected abstract Color OutlineColor { get; }

    protected abstract Color TextForeground { get; }
    protected abstract Color TextBackground { get; }
    protected abstract Color DocumentTitleForeground { get; }
    protected abstract Color DocumentTitleBackground { get; }
    protected abstract Color DocumentTitleInvertedForeground { get; }
    protected abstract Color DocumentTitleInvertedBackground { get; }
    protected abstract Color DocumentSubtitleForeground { get; }
    protected abstract Color DocumentSubtitleBackground { get; }
    protected abstract Color TextTForeground { get; }
    protected abstract Color TextTBackground { get; }
    protected abstract Color TextHForeground { get; }
    protected abstract Color TextHBackground { get; }

    public static void Apply(Theme theme)
    {
        SetDefaultColor("BACKGROUND", theme.Background);
        SetDefaultColor("SELECTED", theme.Selected);
        SetDefaultColor("LINE_GRAY", theme.LineGray);
        SetDefaultColor("LINE_GRAY_LIGHT", theme.LineGrayLight);
        SetDefaultColor("DOCUMENT_GRAY", theme.DocumentGray);
        SetDefaultColor("DOCUMENT_BACKGROUND", theme.DocumentBackground);

        SetDefaultColor("BUTTON_ACTIVE_COLOR", theme.ButtonActiveColor);
        SetDefaultColor("BUTTON_ACTIVE_COLOR_INVERTED", theme.ButtonActiveColorInverted);
        SetDefaultColor("BUTTON_DISABLED_COLOR", theme.ButtonDisabledColor);
        SetDefaultColor("BUTTON_DISABLED_LIGHT_COLOR", theme.ButtonDisabledLightColor);

        SetDefaultColor("OUTLINE_COLOR", theme.OutlineColor);

        SetDefaultTextColor("TEXT_COLOR", theme.TextForeground, theme.TextBackground);
        SetDefaultTextColor("DOCUMENT_TITLE_COLOR", theme.DocumentTitleForeground,
            theme.DocumentTitleBackground);
        SetDefaultTextColor("DOCUMENT_TITLE_COLOR_INVERTED", theme.DocumentTitleInvertedForeground,
            theme.DocumentTitleInvertedBackground);
        SetDefaultTextColor("DOCUMENT_SUBTITLE_COLOR", theme.DocumentSubtitleForeground,
            theme.DocumentSubtitleBackground);
        SetDefaultTextColor("TEXT_COLOR_T", theme.TextTForeground, theme.TextTBackground);
        SetDefaultTextColor("TEXT_COLOR_H", theme.TextHForeground, theme.TextHBackground);
    }

    private static void SetDefaultColor(string key, Color color)
    {
        var keyPtr = Pdk.Allocate(key).Offset;

        Functions.SetDefaultColor(keyPtr, Utils.Serialize(color, JsonContext.Default.Color));
    }

    private static void SetDefaultTextColor(string key, Color foreground, Color background)
    {
        var keyPtr = Pdk.Allocate(key).Offset;
        var textColor = new TextColor(foreground, background);
        var textColorPtr = Utils.Serialize(textColor, JsonContext.Default.TextColor);

        Functions.SetDefaultTextColor(keyPtr, textColorPtr);
    }
}