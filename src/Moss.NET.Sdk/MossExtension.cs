using System;
using System.Security.Cryptography.X509Certificates;
using Extism;

namespace Moss.NET.Sdk;

public class MossExtension
{
    public static MossExtension Instance;

    public virtual ExtensionInfo Register(MossState state)
    {
        return null;
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
        Pdk.SetOutputJson(Instance.Register(input), JsonContext.Default.ExtensionInfo);
    }

    public static void Init<T>() where T : MossExtension, new()
    {
        if (Instance is not null)
        {
            throw new InvalidOperationException("Assembly can only have one extension instance.");
        }

        Instance = Activator.CreateInstance<T>();
        SetExtensionInfo();
    }
}