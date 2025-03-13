using System;
using System.Runtime.InteropServices;
using Extism;
using FSharpSample.NET.Sdk;
using FSharpSample.NET.Sdk.FFI;

namespace SamplePlugin;

public class SampleExtension : FSharpSampleExtension
{
    [UnmanagedCallersOnly(EntryPoint = "fsharpsample_extension_register")]
    public static ulong Register()
    {
        Init<SampleExtension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override ExtensionInfo Register(FSharpSampleState state)
    {
        Pdk.Log(LogLevel.Info, "Hello world form sample plugin");

        return new ExtensionInfo([]);
    }

    public override void Unregister()
    {
    }
}