using Extism;
using Moss.NET.Sdk;
using Moss.NET.Sdk.FFI;

namespace SamplePlugin;

public class SampleScreen : Screen
{
    private ulong helloID;
    public override string Name => "SampleScreen";

    public override void PreLoop()
    {
        helloID = DrawingContext.MakeText("Hello, World!", 12);
        DrawingContext.SetText(helloID, "Edited");
    }

    public override void Loop()
    {
        Pdk.Log(LogLevel.Warn, "samplescreen loop calling");

        DrawingContext.DrawRect(Color.Red, 10, 10, 10, 10);
        DrawingContext.SetFont(helloID, Defaults.GetDefaultValue<string>("CUSTOM_FONT"), 25);
        DrawingContext.DisplayText(helloID, new Rect(100, 100, 100, 100));
    }
}