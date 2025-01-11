using System;

namespace Moss.NET.Sdk;

public class MossExtension
{
    internal static MossExtension Instance;

    public static void Load<T>() where T : MossExtension, new()
    {
        if (Instance is not null)
        {
            throw new InvalidOperationException("Assembly can only have one extension instance.");
        }

        Instance = Activator.CreateInstance<T>();
    }

    public virtual ExtensionInfo Register(MossState state)
    {
        return null;
    }

    public virtual void Unregister()
    {

    }
}