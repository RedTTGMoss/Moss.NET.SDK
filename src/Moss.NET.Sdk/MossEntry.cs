using System.Runtime.InteropServices;
using Extism;

namespace Moss.NET.Sdk;

public static class MossEntry
{
    [UnmanagedCallersOnly(EntryPoint = "moss_register")]
    public static ulong Register()
    {
        var input = Pdk.GetInputJson(JsonContext.Default.MossState);
        
        Pdk.SetOutputJson(MossExtension.Instance.Register(input), JsonContext.Default.ExtensionInfo);

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "moss_unregister")]
    public static ulong Unregister()
    {
        MossExtension.Instance.Unregister();
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_loop")]
    public static ulong ExtensionLoop()
    {
        var state = Pdk.GetInputJson(JsonContext.Default.MossState);

        MossExtension.Instance.ExtensionLoop(state);

        return 0;
    }
}