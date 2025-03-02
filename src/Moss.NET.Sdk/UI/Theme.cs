namespace Moss.NET.Sdk.UI;

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
        Defaults.SetDefaultColor("BACKGROUND", theme.Background);
        Defaults.SetDefaultColor("SELECTED", theme.Selected);
        Defaults.SetDefaultColor("LINE_GRAY", theme.LineGray);
        Defaults.SetDefaultColor("LINE_GRAY_LIGHT", theme.LineGrayLight);
        Defaults.SetDefaultColor("DOCUMENT_GRAY", theme.DocumentGray);
        Defaults.SetDefaultColor("DOCUMENT_BACKGROUND", theme.DocumentBackground);

        Defaults.SetDefaultColor("BUTTON_ACTIVE_COLOR_INVERTED", theme.ButtonActiveColorInverted);
        Defaults.SetDefaultColor("BUTTON_DISABLED_COLOR", theme.ButtonDisabledColor);
        Defaults.SetDefaultColor("BUTTON_DISABLED_LIGHT_COLOR", theme.ButtonDisabledLightColor);
        Defaults.SetDefaultColor("BUTTON_ACTIVE_COLOR", theme.ButtonActiveColor);

        Defaults.SetDefaultColor("OUTLINE_COLOR", theme.OutlineColor);

        Defaults.SetDefaultTextColor("TEXT_COLOR", theme.TextForeground, theme.TextBackground);
        Defaults.SetDefaultTextColor("DOCUMENT_TITLE_COLOR", theme.DocumentTitleForeground,
            theme.DocumentTitleBackground);
        Defaults.SetDefaultTextColor("DOCUMENT_TITLE_COLOR_INVERTED", theme.DocumentTitleInvertedForeground,
            theme.DocumentTitleInvertedBackground);
        Defaults.SetDefaultTextColor("DOCUMENT_SUBTITLE_COLOR", theme.DocumentSubtitleForeground,
            theme.DocumentSubtitleBackground);
        Defaults.SetDefaultTextColor("TEXT_COLOR_T", theme.TextTForeground, theme.TextTBackground);
        Defaults.SetDefaultTextColor("TEXT_COLOR_H", theme.TextHForeground, theme.TextHBackground);
    }
}