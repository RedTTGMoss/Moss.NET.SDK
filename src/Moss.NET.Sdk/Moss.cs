using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.UI;

namespace Moss.NET.Sdk;

public static class Moss
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_get_state")]
    private static extern ulong GetMossState(); //-> MossState

    [DllImport(Functions.DLL, EntryPoint = "moss_em_register_extension_button")]
    private static extern void RegisterExtensionButton(ulong buttonPtr);

    public static MossState GetState()
    {
        return GetMossState().Get<MossState>();
    }

    public static void RegisterExtensionButton(ContextButton button)
    {
        RegisterExtensionButton(button.GetPointer());
    }
}