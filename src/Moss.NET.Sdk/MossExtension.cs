using System;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public class MossExtension
{
    internal static MossExtension Instance;

    public virtual ExtensionInfo Register(MossState state)
    {
        return new ExtensionInfo([]);
    }

    public virtual void Unregister()
    {
    }

    public virtual void ExtensionLoop(MossState state)
    {
    }

    private static void SetExtensionInfo()
    {
        var input = Pdk.GetInputJson(FFI.JsonContext.Default.MossState);
        Pdk.SetOutputJson(Instance.Register(input), FFI.JsonContext.Default.ExtensionInfo);
    }

    public static void Init<T>() where T : MossExtension, new()
    {
        if (Instance is not null) throw new InvalidOperationException("Assembly can only have one extension instance.");

        Instance = Activator.CreateInstance<T>();
        SetExtensionInfo();
    }
}