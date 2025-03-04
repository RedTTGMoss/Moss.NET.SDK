using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.NEW;

namespace Moss.NET.Sdk.Core;

public static class InternalFunctions
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_export_statistical_data")]
    public static extern void ExportStatisticalData();

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_export")]
    private static extern void ExportDocument(ulong documentUuidPtr);

    public static void ExportDocument(string documentUuid)
    {
        ExportDocument(GetAccessor(documentUuid).GetPointer());
    }

    internal static Accessor GetAccessor(string uuid) => new() { Type = AccessorType.APIDocument, Uuid = uuid};
}