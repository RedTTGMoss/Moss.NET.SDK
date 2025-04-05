using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.Storage;

public class Document : StorageItem<Document>
{
    public Document(string name, string? parent = null)
    {
        ID = StorageFunctions.NewMetadata(name, RMDocumentType.DocumentType, parent);

        var uuid = NewNotebook(name, parent);
        Metadata = Metadata.Get(uuid);
    }

    public Document()
    {
    }

    public StandaloneId ID { get; set; }

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_notebook")]
    private static extern ulong NewNotebook(ulong newDocPtr);

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