using System;
using System.Text.Json;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public class MossExtension
{
    internal static MossExtension? Instance;

    public virtual ExtensionInfo Register(MossState state)
    {
        return new ExtensionInfo();
    }

    public virtual void Unregister()
    {
    }

    public virtual void ExtensionLoop(MossState state)
    {
    }

    private static void SetExtensionInfo()
    {
        var input = Pdk.GetInputJson(JsonContext.Default.MossState);
        var extensionInfo = Instance!.Register(input!);
        extensionInfo.Files = Assets.Expose();

        #if DEBUG
        Pdk.Log(LogLevel.Info, "output: " + JsonSerializer.Serialize(extensionInfo, JsonContext.Default.ExtensionInfo));
        #endif

        Pdk.SetOutputJson(extensionInfo, JsonContext.Default.ExtensionInfo);
    }

    public static void Init<T>() where T : MossExtension, new()
    {
        if (Instance is not null) throw new InvalidOperationException("Assembly can only have one extension instance.");

        Instance = Activator.CreateInstance<T>();
        SetExtensionInfo();
    }
}