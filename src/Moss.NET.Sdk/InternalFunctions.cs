using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public class InternalFunctions
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_export_statistical_data")]
    public static extern void ExportStatisticalData();
}