using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Moss.NET.Sdk.API;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;
using Moss.NET.Sdk.NEW;

namespace Moss.NET.Sdk.Storage;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
public static class StorageFunctions
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_get_all")]
    private static extern ulong Get(ulong accessorPtr); // -> ConfigGet[T]

    [DllImport(Functions.DLL, EntryPoint = "moss_api_get")]
    private static extern ulong Get(ulong accessorPtr, ulong keyPtr); // -> ConfigGet[T]

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_pdf")]
    private static extern ulong NewPdf(ulong newPdfPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_epub")]
    private static extern ulong NewEpub(ulong newEpPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_metadata_new")]
    private static extern ulong NewMetadata(ulong metadataNewPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_content_new_notebook")]
    private static extern ulong MossNewContentNotebook(ulong pageCount); // -> int

    [DllImport(Functions.DLL, EntryPoint = "moss_api_content_new_pdf")]
    private static extern ulong MossNewContentPdf(); // -> int

    [DllImport(Functions.DLL, EntryPoint = "moss_api_content_new_epub")]
    private static extern ulong MossNewContentEpub(); // -> int

    [DllImport(Functions.DLL, EntryPoint = "moss_api_delete")]
    private static extern ulong Delete(ulong accessorPtr, ulong callbackPtr, int unload); // ->taskid

    [DllImport(Functions.DLL, EntryPoint = "moss_api_upload")]
    private static extern ulong Upload(ulong accessorPtr, ulong callbackPtr, int unload);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_upload_many_documents")]
    private static extern ulong UploadManyDocuments(ulong accessorListPtr, ulong callbackPtr, int unload);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_delete_many_documents")]
    private static extern ulong DeleteManyDocuments(ulong accessorListPtr, ulong callbackPtr, int unload);

    public static ulong Delete(Accessor accessor, string callback, bool unload = false)
    {
        return Delete(accessor.GetPointer(), callback.GetPointer(), unload ? 1 : 0);
    }

    public static ulong Upload(Accessor accessor, string callback, bool unload = false)
    {
        return Upload(accessor.GetPointer(), callback.GetPointer(), unload ? 1 : 0);
    }

    public static void UploadManyDocuments(IEnumerable<Accessor> accessors, string callback, bool unload = false)
    {
        UploadManyDocuments(accessors.ToArray().GetPointer(), callback.GetPointer(), unload ? 1 : 0);
    }

    public static void DeleteManyDocuments(IEnumerable<Accessor> accessors, string callback, bool unload = false)
    {
        DeleteManyDocuments(accessors.ToArray().GetPointer(), callback.GetPointer(), unload ? 1 : 0);
    }

    public static void UploadManyDocuments(IEnumerable<Document> documents, string callback, bool unload = false)
    {
        var accessors = documents.Select(d => new Accessor
        {
            Type = AccessorType.APIDocument,
            Uuid = d.Metadata.Accessor.Uuid
        });

        UploadManyDocuments(accessors, callback, unload);
    }

    public static void DeleteManyDocuments(IEnumerable<Document> documents, string callback, bool unload = false)
    {
        var accessors = documents.Select(d => new Accessor
        {
            Type = AccessorType.APIDocument,
            Uuid = d.Metadata.Accessor.Uuid
        });

        DeleteManyDocuments(accessors, callback, unload);
    }

    public static T GetApiDocumentMetadata<T>(string uuid, string key)
    {
        var accessor = new Accessor
        {
            Type = AccessorType.APIDocumentMetadata,
            Uuid = uuid
        };

        return Get<T>(accessor, key);
    }

    public static T Get<T>(Accessor accessor)
    {
        var resultPtr = Get(accessor.GetPointer());
        var result = resultPtr.Get<ConfigGetD>();

        return result.value.GetValue<T>();
    }

    public static T Get<T>(Accessor accessor, string key)
    {
        var resultPtr = Get(accessor.GetPointer(), key.GetPointer());
        var result = resultPtr.Get<ConfigGetD>();

        return result.value.GetValue<T>();
    }

    /// <summary>
    /// Creates a new PDF document from a file.
    /// </summary>
    /// <param name="name">The name of the new PDF document.</param>
    /// <param name="file">The file path of the PDF document.</param>
    /// <param name="parent">The parent document UUID (optional).</param>
    /// <returns>The UUID of the new PDF document.</returns>
    public static string NewPdf(string name, string file, string? parent = null)
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

    public static string NewPdf(string name, Base64 data, string? parent = null)
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

    /// <summary>
    /// Creates a new EPUB document from a file.
    /// </summary>
    /// <param name="name">The name of the new EPUB document.</param>
    /// <param name="file">The file path of the EPUB document.</param>
    /// <param name="parent">The parent document UUID (optional).</param>
    /// <returns>The UUID of the new EPUB document.</returns>
    public static string NewEpub(string name, string file, string? parent = null)
    {
        var notebook = new DocumentNewEpub
        {
            Name = name,
            Parent = parent,
            EpubFile = file,
            Accessor = new Accessor
            {
                Type = AccessorType.APIDocument
            }
        };

        return NewEpub(notebook.GetPointer()).ReadString();
    }

    public static string NewEpub(string name, Base64 data, string? parent = null)
    {
        var notebook = new DocumentNewEpub
        {
            Name = name,
            Parent = parent,
            EpubData = data,
            Accessor = new Accessor
            {
                Type = AccessorType.APIDocument
            }
        };

        return NewEpub(notebook.GetPointer()).ReadString();
    }

    /// <summary>
    /// Create a new metadata object
    /// </summary>
    /// <param name="name">The visible name</param>
    /// <param name="type">Does this metadata corresponds to a notebook or a collection?</param>
    /// <param name="parent">The parent collection</param>
    /// <returns>An id to work with later</returns>
    internal static StandaloneId NewMetadata(string name, RMDocumentType type, string? parent = null)
    {
        var md = new MetadataNew
        {
            Name = name,
            DocumentType = type,
            Parent = parent
        };

        return NewMetadata(md.GetPointer());
    }

    /// <summary>
    /// This function creates a content object for a blank notebook with a specified page count.
    /// You must pass at minimum one page! Returns standalone content id
    /// </summary>
    /// <param name="pageCount">The number of pages in the notebook</param>
    /// <returns>The content id</returns>
    public static StandaloneId NewContentNotebook(ulong pageCount)
    {
        return MossNewContentNotebook(pageCount);
    }

    public static StandaloneId NewContentPdf()
    {
        return MossNewContentPdf();
    }

    public static StandaloneId NewContentEpub()
    {
        return MossNewContentEpub();
    }
}