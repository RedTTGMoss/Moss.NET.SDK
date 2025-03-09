using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;

namespace SamplePlugin;

public class Extension : MossExtension
{
    [UnmanagedCallersOnly(EntryPoint = "archiver_extension_register")]
    public static ulong Register()
    {
        Init<Extension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override void Register(MossState state)
    {
        Pdk.Log(LogLevel.Info, "Hello world form sample plugin");
    }

    public override void Unregister()
    {
    }
}