using Moss.NET.Sdk;
using Moss.NET.Sdk.Drawing;

namespace SamplePlugin;

public class DarkTheme : Theme
{
    protected override Color Background => Color.FromHex("#3f3f3f");
    protected override Color Selected => new(38, 79, 120);
    protected override Color LineGray => new(40, 40, 40);
    protected override Color LineGrayLight => new(60, 60, 60);
    protected override Color DocumentGray => new(30, 30, 30);
    protected override Color DocumentBackground => new(25, 25, 25);
    protected override Color ButtonActiveColor => Color.FromHex("#482cdd");
    protected override Color ButtonActiveColorInverted => new(255, 255, 255);
    protected override Color ButtonDisabledColor => new(100, 100, 100);
    protected override Color ButtonDisabledLightColor => new(120, 120, 120);
    protected override Color OutlineColor => Color.FromHex("#030ad8");

    protected override Color TextForeground => Color.FromHex("#121212");
    protected override Color TextBackground => Background;
    protected override Color DocumentTitleForeground => Color.FromHex("#121212");
    protected override Color DocumentTitleBackground => Background;
    protected override Color DocumentTitleInvertedForeground => new(18, 18, 18);
    protected override Color DocumentTitleInvertedBackground => Background;
    protected override Color DocumentSubtitleForeground => Color.FromHex("#121212");
    protected override Color DocumentSubtitleBackground => Background;
    protected override Color TextTForeground => Color.FromHex("#121212");
    protected override Color TextTBackground => Background;
    protected override Color TextHForeground => new(150, 150, 150);
    protected override Color TextHBackground => Background;
}