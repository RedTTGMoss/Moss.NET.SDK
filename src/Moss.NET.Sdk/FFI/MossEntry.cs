using System.Runtime.InteropServices;
using Extism;

namespace Moss.NET.Sdk.FFI;

internal class MossEntry
{
    private static LoggerInstance _logger = Log.GetLogger<MossEntry>();

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_unregister")]
    public static ulong Unregister()
    {
        MossExtension.Instance!.Unregister();
        _logger.Info("unregistered extension");
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_loop")]
    public static ulong ExtensionLoop()
    {
        var state = Pdk.GetInputJson(JsonContext.Default.MossState)!;

        MossExtension.Instance!.ExtensionLoop(state);

        return 0;
    }
}