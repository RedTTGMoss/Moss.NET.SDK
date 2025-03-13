using System.Runtime.InteropServices;
using System.Text.Json;
using Extism;

namespace Moss.NET.Sdk.FFI;

internal class MossEntry
{
    private static LoggerInstance _logger = Log.GetLogger<MossEntry>();

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_register")]
    public static ulong Register()
    {
        var inputJson = Pdk.GetInputJson(JsonContext.Default.MossState);
        MossExtension.Instance!.Register(inputJson!);

        Pdk.SetOutput(JsonSerializer.Serialize(new ExtensionInfo
        {
            Files = Assets.Expose()
        }, JsonContext.Default.ExtensionInfo));

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_unregister")]
    public static ulong Unregister()
    {
        MossExtension.Instance?.Unregister();

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_loop")]
    public static ulong ExtensionLoop()
    {
        var state = Pdk.GetInputJson(JsonContext.Default.MossState)!;

        MossExtension.Instance?.ExtensionLoop(state);

        return 0;
    }
}