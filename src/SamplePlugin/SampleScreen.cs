using Moss.NET.Sdk.UI;
using Moss.NET.Sdk.UI.Widgets;

namespace SamplePlugin;

public class SampleScreen : Screen
{
    private Label _hello;
    private Rectangle _rectangle;
    public override string Name => "SampleScreen";

    public override void PreLoop()
    {
        _hello = new Label("Hello, World!", 12, 100,100);
        _rectangle = new Rectangle(Color.Red, 10, 10, 10, 10);

        _hello.FontSize = 12;
        _hello.Text = "Edited";

        AddWidget(_hello);
        AddWidget(_rectangle);

        ScreenManager.SetValue("hello", true);

        ScreenManager.GetValue<bool>("hello");
    }

    protected override void OnLoop()
    {

    }
}