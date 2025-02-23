using System.Runtime.InteropServices;
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
    }

    public override void Loop()
    {
        Pdk.Log(LogLevel.Warn, "samplescreen loop calling");

        DrawingContext.DrawRect(Color.Red, 10, 10, 10, 10);
        DrawingContext.DisplayText(helloID, new Rect(100, 100, 100, 100));
    }
}