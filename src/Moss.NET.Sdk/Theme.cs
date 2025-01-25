using System;
using Extism;

namespace Moss.NET.Sdk;

public abstract class Theme
{
    public abstract Color Background { get; }
    public abstract Color Selected { get; }
    public abstract Color LineGray { get; }
    public abstract Color LineGrayLight { get; }
    public abstract Color DocumentGray { get; }
    public abstract Color DocumentBackground { get; }
    public abstract Color ButtonActiveColor { get; }
    public abstract Color ButtonActiveColorInverted { get; }
    public abstract Color ButtonDisabledColor { get; }
    public abstract Color ButtonDisabledLightColor { get; }
    public abstract Color OutlineColor { get; }

    public abstract Color TextForeground { get; }
    public abstract Color TextBackground { get; }
    public abstract Color DocumentTitleForeground { get; }
    public abstract Color DocumentTitleBackground { get; }
    public abstract Color DocumentTitleInvertedForeground { get; }
    public abstract Color DocumentTitleInvertedBackground { get; }
    public abstract Color DocumentSubtitleForeground { get; }
    public abstract Color DocumentSubtitleBackground { get; }
    public abstract Color TextTForeground { get; }
    public abstract Color TextTBackground { get; }
    public abstract Color TextHForeground { get; }
    public abstract Color TextHBackground { get; }

    public static void Apply(Theme theme)
    {
        SetDefaultColor(ColorKey.BACKGROUND, theme.Background);
        SetDefaultColor(ColorKey.SELECTED, theme.Selected);
        SetDefaultColor(ColorKey.LINE_GRAY, theme.LineGray);
        SetDefaultColor(ColorKey.LINE_GRAY_LIGHT, theme.LineGrayLight);
        SetDefaultColor(ColorKey.DOCUMENT_GRAY, theme.DocumentGray);
        SetDefaultColor(ColorKey.DOCUMENT_BACKGROUND, theme.DocumentBackground);

        SetDefaultColor(ColorKey.BUTTON_ACTIVE_COLOR, theme.ButtonActiveColor);
        SetDefaultColor(ColorKey.BUTTON_ACTIVE_COLOR_INVERTED, theme.ButtonActiveColorInverted);
        SetDefaultColor(ColorKey.BUTTON_DISABLED_COLOR, theme.ButtonDisabledColor);
        SetDefaultColor(ColorKey.BUTTON_DISABLED_LIGHT_COLOR, theme.ButtonDisabledLightColor);

        SetDefaultColor(ColorKey.OUTLINE_COLOR, theme.OutlineColor);

        SetDefaultTextColor(TextColorKey.TEXT_COLOR, theme.TextForeground, theme.TextBackground);
        SetDefaultTextColor(TextColorKey.DOCUMENT_TITLE_COLOR, theme.DocumentTitleForeground, theme.DocumentTitleBackground);
        SetDefaultTextColor(TextColorKey.DOCUMENT_TITLE_COLOR_INVERTED, theme.DocumentTitleInvertedForeground, theme.DocumentTitleInvertedBackground);
        SetDefaultTextColor(TextColorKey.DOCUMENT_SUBTITLE_COLOR, theme.DocumentSubtitleForeground, theme.DocumentSubtitleBackground);
        SetDefaultTextColor(TextColorKey.TEXT_COLOR_T, theme.TextTForeground, theme.TextTBackground);
        SetDefaultTextColor(TextColorKey.TEXT_COLOR_H, theme.TextHForeground, theme.TextHBackground);
    }

    private static void SetDefaultColor(ColorKey key, Color color)
    {
        Pdk.Log(LogLevel.Info, "try to set color " + key);
        FFI.Functions.SetDefaultColor(Pdk.Allocate(key.ToString()).Offset, color.R, color.G, color.B, color.A);
    }

    private static void SetDefaultTextColor(TextColorKey key, Color foreground, Color background)
    {
        Pdk.Log(LogLevel.Info, "try to set text color " + key);
        FFI.Functions.SetDefaultTextColor(Pdk.Allocate(key.ToString()).Offset,
            foreground.R, foreground.G, foreground.B,
            background.R, background.G, background.B
        );
    }
}