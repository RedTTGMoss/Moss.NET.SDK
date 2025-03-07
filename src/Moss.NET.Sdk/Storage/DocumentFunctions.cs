using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.Storage;

internal static class DocumentFunctions
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_unload_files")]
    internal static extern void UnloadFiles(ulong accessorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_randomize_uuids")]
    internal static extern ulong RandomizeUuids(ulong accessorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_load_files_from_cache")]
    internal static extern void LoadFilesFromCache(ulong accessorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_duplicate")]
    internal static extern ulong DuplicateDocument(ulong accessorPtr); // -> string

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_ensure_download")]
    internal static extern void EnsureDownload(ulong accessorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_ensure_download_and_callback")]
    internal static extern void EnsureDownload(ulong accessorPtr, ulong callbackPtr);
}