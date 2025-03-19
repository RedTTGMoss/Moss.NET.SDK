using System;
using System.Text.Json;
using Extism;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.Scheduler;

namespace Moss.NET.Sdk;

public class MossExtension
{
    private static MossExtension? _instance;

    internal static MossExtension? Instance
    {
        get
        {
            if (_instance is null) throw new InvalidOperationException("Extension is not initialized.");

            return _instance;
        }
        set => _instance = value;
    }

    public virtual void Register(MossState state)
    {

    }

    public virtual void Unregister()
    {
    }

    public virtual void ExtensionLoop(MossState state)
    {
    }

    public static void Init<T>() where T : MossExtension, new()
    {
        if (_instance is not null) throw new InvalidOperationException("Assembly can only have one extension instance.");

        _instance = Activator.CreateInstance<T>();

        var input = Pdk.GetInputJson(JsonContext.Default.MossState);
        _instance!.Register(input!);
        var extensionInfo = new ExtensionInfo()
        {
            Files = Assets.Expose()
        };

        var output = JsonSerializer.Serialize(extensionInfo, JsonContext.Default.ExtensionInfo);
#if DEBUG
        Pdk.Log(LogLevel.Info, "output: " + output);
#endif
        TaskScheduler.LoadTaskInformation();

        Pdk.SetOutput(output);
    }
}