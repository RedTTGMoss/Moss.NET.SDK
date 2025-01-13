using System;
using Extism;
using Moss.NET.Sdk;

namespace SamplePlugin;

public class SampleExtension : MossExtension
{
    public static void Main()
    {
        Load<SampleExtension>();
        Pdk.Log(LogLevel.Info, "main sample extension");
    }

    public override ExtensionInfo Register(MossState state)
    {
        Pdk.Log(LogLevel.Info, "registered sample extension");
        return new ExtensionInfo([]);
    }

    public override void Unregister()
    {
        Pdk.Log(LogLevel.Info, "unregistered sample extension");
    }
}