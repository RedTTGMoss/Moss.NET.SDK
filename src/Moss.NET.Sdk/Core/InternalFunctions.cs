using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;
using Moss.NET.Sdk.NEW;

namespace Moss.NET.Sdk.Core;

public static class InternalFunctions
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_export_statistical_data")]
    public static extern void ExportStatisticalData();

    [DllImport(Functions.DLL, EntryPoint = "moss_em_loader_progress")]
    private static extern ulong LoaderProgress(); // -> ConfigGet[f64]

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_export")]
    private static extern void ExportDocument(ulong documentUuidPtr);

    public static void ExportDocument(string documentUuid)
    {
        ExportDocument(GetAccessor(documentUuid).GetPointer());
    }

    public static double GetLoaderProgress()
    {
        return LoaderProgress().Get<ConfigGetD>().value.GetValue<double>();
    }

    internal static Accessor GetAccessor(string uuid) => new() { Type = AccessorType.APIDocument, Uuid = uuid};
}