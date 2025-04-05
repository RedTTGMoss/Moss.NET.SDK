using Moss.NET.Sdk;
using Moss.NET.Sdk.UI;
using Moss.NET.Sdk.UI.Widgets;

namespace SdkTesterPlugin;

public class SampleScreen : Screen
{
    private static readonly LoggerInstance _logger = Log.GetLogger<SampleScreen>();

    private Label _hello;
    private Rectangle _rectangle;
    public override string Name => "SampleScreen";

    public override void PreLoop()
    {
        _hello = new Label("Hello, World!", 12, 100, 100);
        _rectangle = new Rectangle(Color.Red, 10, 10, 10, 10);

        _hello.FontSize = 12;
        _hello.Text = "Edited";

        AddWidget(_hello);
        AddWidget(_rectangle);

        SetValue("hello", true);

        GetValue<bool>("hello");
    }

    protected override void OnLoop()
    {
    }
}