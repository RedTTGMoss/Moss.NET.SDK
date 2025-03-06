using System.Runtime.InteropServices;
using Moss.NET.Sdk.API;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;
using Moss.NET.Sdk.NEW;

namespace Moss.NET.Sdk.Storage;

public partial class Document
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_duplicate")]
    private static extern ulong DuplicateDocument(ulong accessorPtr); // -> string

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_ensure_download")]
    private static extern void EnsureDownload(ulong accessorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_ensure_download_and_callback")]
    private static extern void EnsureDownload(ulong accessorPtr, ulong callbackPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_notebook")]
    private static extern ulong NewNotebook(ulong newDocPtr);

    private static Metadata GetApiDocumentMetadata(string uuid)
    {
        var accessor = new Accessor()
        {
            Type = AccessorType.APIDocumentMetadata,
            Uuid = uuid
        };

        return StorageFunctions.Get<Metadata>(accessor);
    }

    private static string NewNotebook(string name, string? parent = null)
    {
        var notebook = new DocumentNewNotebook
        {
            Name = name,
            Parent = parent,
            Accessor = new Accessor
            {
                Type = AccessorType.APIDocument
            },
            NotebookFiles = []
        };

        return NewNotebook(notebook.GetPointer()).ReadString();
    }
}