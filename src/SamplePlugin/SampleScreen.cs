using Moss.NET.Sdk;
using Moss.NET.Sdk.UI;

namespace SamplePlugin;

public class SampleScreen : Screen
{
    private TextWidget helloWidget;
    private RectWidget rectWidget;
    public override string Name => "SampleScreen";

    public override void PreLoop()
    {
        helloWidget = new TextWidget("Hello, World!", 12, 100,100,100,50);
        rectWidget = new RectWidget(Color.Red, 10,10,10,10);

        helloWidget.FontSize = 12;
        helloWidget.Text = "Edited";

        AddWidget(helloWidget);
        AddWidget(rectWidget);

        ScreenManager.SetValue("hello", true);
    }

    protected override void OnLoop()
    {
        ScreenManager.GetValue<bool>("hello");
    }
}