using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public class Moss
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_get_state")]
    private static extern ulong GetMossState(); //-> MossState

    [DllImport(Functions.DLL, EntryPoint = "moss_em_register_extension_button")]
    private static extern void RegisterExtensionButton(ulong buttonPtr);

    public static MossState GetState()
    {
        return Utils.Deserialize(GetMossState(), JsonContext.Default.MossState);
    }

    public static void RegisterExtensionButton(ContextButton button)
    {
        RegisterExtensionButton(Utils.Serialize(button, JsonContext.Default.ContextButton));
    }
}