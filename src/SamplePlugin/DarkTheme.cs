using Moss.NET.Sdk;

namespace SamplePlugin;

public class DarkTheme : Theme
{
    public override Color Background => Color.White;
    public override Color Selected => new(38, 79, 120);
    public override Color LineGray => new(40, 40, 40);
    public override Color LineGrayLight => new(60, 60, 60);
    public override Color DocumentGray => new(30, 30, 30);
    public override Color DocumentBackground => new(25, 25, 25);
    public override Color ButtonActiveColor => new(0, 120, 215);
    public override Color ButtonActiveColorInverted => new(255, 255, 255);
    public override Color ButtonDisabledColor => new(100, 100, 100);
    public override Color ButtonDisabledLightColor => new(120, 120, 120);
    public override Color OutlineColor => new(61, 61, 61);

    public override Color TextForeground => Color.Red;
    public override Color TextBackground => Background;
    public override Color DocumentTitleForeground => new(230, 230, 230);
    public override Color DocumentTitleBackground => Background;
    public override Color DocumentTitleInvertedForeground => new(18, 18, 18);
    public override Color DocumentTitleInvertedBackground => Background;
    public override Color DocumentSubtitleForeground => new(180, 180, 180);
    public override Color DocumentSubtitleBackground => Background;
    public override Color TextTForeground => new(200, 200, 200);
    public override Color TextTBackground => Background;
    public override Color TextHForeground => new(150, 150, 150);
    public override Color TextHBackground => Background;
}