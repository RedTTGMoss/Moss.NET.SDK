using System;
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
         Pdk.Log(LogLevel.Info, "extension started");
    }

    public override ExtensionInfo Register(MossState state)
    {
        Pdk.Log(LogLevel.Info, "registered sample extension");

        Config.Set("theme", "dark");
        Pdk.Log(LogLevel.Info, "before applying theme");
        try
        {
            Theme.Apply(new DarkTheme());
        }
        catch (Exception ex)
        {
            Pdk.Log(LogLevel.Error, ex.Message);
        }

        Pdk.Log(LogLevel.Info, "after applying theme");

        return new ExtensionInfo([]);
    }

    public override void Unregister()
    {
        Pdk.Log(LogLevel.Info, "unregistered sample extension");
    }
}