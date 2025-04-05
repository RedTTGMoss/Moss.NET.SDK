using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.Core;

public static class InternalFunctions
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_export_statistical_data")]
    public static extern void ExportStatisticalData();

    [DllImport(Functions.DLL, EntryPoint = "moss_em_loader_progress")]
    private static extern ulong LoaderProgress(); // -> ConfigGet[f64]

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_export")]
    private static extern void ExportDocument(ulong documentUuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_get_root")]
    private static extern ulong GetRoot(); // -> RootInfo

    [DllImport(Functions.DLL, EntryPoint = "moss_api_new_file_sync_progress")]
    public static extern ulong NewFileSyncProgress();

    [DllImport(Functions.DLL, EntryPoint = "moss_api_new_document_sync_progress")]
    private static extern ulong NewDocumentSyncProgress(ulong accessorPtr, ulong documentUuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_content_new_epub")]
    public static extern ulong NewContentEpub(); // -> int

    [DllImport(Functions.DLL, EntryPoint = "moss_api_content_new_pdf")]
    public static extern ulong NewContentPdf(); // -> int

    [DllImport(Functions.DLL, EntryPoint = "moss_api_content_new_notebook")]
    public static extern ulong NewContentNotebook(ulong pageCount); // -> int

    [DllImport(Functions.DLL, EntryPoint = "moss_api_spread_event")]
    private static extern void SpreadEvent(ulong accessorPtr);

    public static void SpreadEvent(Accessor accessor)
    {
        SpreadEvent(accessor.GetPointer());
    }

    public static ulong NewDocumentSyncProgress(Accessor accessor)
    {
        return NewDocumentSyncProgress(accessor.GetPointer(), accessor.Uuid!.GetPointer());
    }

    public static RootInfo GetRootInfo()
    {
        return GetRoot().Get<RootInfo>();
    }

    public static void ExportDocument(string documentUuid)
    {
        ExportDocument(GetAccessor(documentUuid).GetPointer());
    }

    public static double GetLoaderProgress()
    {
        return LoaderProgress().Get<ConfigGetD>().value.GetValue<double>();
    }

    internal static Accessor GetAccessor(string uuid)
    {
        return new Accessor { Type = AccessorType.APIDocument, Uuid = uuid };
    }
}