using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.Storage;

public class PdfNotebook : StorageItem<PdfNotebook>
{
    public PdfNotebook()
    {
    }

    public PdfNotebook(string name, string file, string? parent = null)
    {
        var uuid = NewPdf(name, file, parent);
        Metadata = Metadata.Get(uuid);
    }

    public PdfNotebook(string name, Base64 data, string? parent = null)
    {
        var uuid = NewPdf(name, data, parent);
        Metadata = Metadata.Get(uuid);
    }

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_pdf")]
    private static extern ulong NewPdf(ulong newPdfPtr);

    private static string NewPdf(string name, string file, string? parent = null)
    {
        var notebook = new DocumentNewPdf
        {
            Name = name,
            Parent = parent,
            PdfFile = file,
            Accessor = new Accessor
            {
                Type = AccessorType.APIDocument
            }
        };

        return NewPdf(notebook.GetPointer()).ReadString();
    }

    private static string NewPdf(string name, Base64 data, string? parent = null)
    {
        var notebook = new DocumentNewPdf
        {
            Name = name,
            Parent = parent,
            PdfData = data,
            Accessor = new Accessor
            {
                Type = AccessorType.APIDocument
            }
        };

        return NewPdf(notebook.GetPointer()).ReadString();
    }
}