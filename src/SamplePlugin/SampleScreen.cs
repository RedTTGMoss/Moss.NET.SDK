using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;

namespace SamplePlugin;

public class SampleScreen : IScreen
{
    public static string Name => "SampleScreen";

    public static void Loop()
    {
        Pdk.Log(LogLevel.Warn, "samplescreen loop calling");
    }
}