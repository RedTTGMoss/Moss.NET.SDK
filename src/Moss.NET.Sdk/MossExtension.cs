using System;
using System.Text.Json;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public class MossExtension
{
    internal static MossExtension? Instance;

    public virtual void Register(MossState state)
    {

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
        Instance!.Register(input!);
        var extensionInfo = new ExtensionInfo()
        {
            Files = Assets.Expose()
        };

        var output = JsonSerializer.Serialize(extensionInfo, JsonContext.Default.ExtensionInfo);
#if DEBUG
            Pdk.Log(LogLevel.Info, "output: " + output);
#endif

        Pdk.SetOutput(output);
    }

    public static void Init<T>() where T : MossExtension, new()
    {
        if (Instance is not null) throw new InvalidOperationException("Assembly can only have one extension instance.");

        Instance = Activator.CreateInstance<T>();
        SetExtensionInfo();
    }
}