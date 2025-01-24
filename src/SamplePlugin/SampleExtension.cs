using System;
using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;

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

        return new ExtensionInfo([]);
    }

    public override void Unregister()
    {
        Pdk.Log(LogLevel.Info, "unregistered sample extension");
    }
}