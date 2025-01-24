using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;
using Moss.NET.Sdk.FFI;

namespace SamplePlugin;

public class SampleExtension : MossExtension
{
    [UnmanagedCallersOnly(EntryPoint = "moss_extension_register")]
    public static ulong Register()
    {
        Init<SampleExtension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override ExtensionInfo Register(MossState state)
    {
        Pdk.Log(LogLevel.Info, "registered sample extension");

        Config.SetDefaultColor(ColorKey.BACKGROUND, Color.Red);
        Config.SetDefaultColor(ColorKey.SELECTED, Color.Green);

        Config.SetDefaultTextColor(TextColorKey.TEXT_COLOR, Color.Green);

        return new ExtensionInfo([]);
    }

    public override void Unregister()
    {
        Pdk.Log(LogLevel.Info, "unregistered sample extension");
    }
}