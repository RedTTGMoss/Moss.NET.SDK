using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public class Moss
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_get_state")]
    private static extern ulong GetMossState(); //-> MossState

    public static MossState GetState()
    {
        return Utils.Deserialize<MossState>(GetMossState(), JsonContext.Default.MossState);
    }
}