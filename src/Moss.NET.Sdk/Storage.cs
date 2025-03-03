using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.API;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
public static class Storage
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_metadata_get_all")]
    private static extern ulong GetApiDocumentMetadata(ulong uuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_duplicate")]
    private static extern ulong DuplicateDocument(ulong uuidPtr); // -> string

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_notebook")]
    private static extern ulong NewNotebook(ulong newDocPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_pdf")]
    private static extern ulong NewPdf(ulong newPdfPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_epub")]
    private static extern ulong NewEpub(ulong newEpPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_randomize_uuids")]
    private static extern ulong RandomizeUuids(ulong uuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_unload_files")]
    private static extern void UnloadFiles(ulong uuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_ensure_download")]
    private static extern void EnsureDownload(ulong uuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_load_files_from_cache")]
    private static extern void LoadFilesFromCache(ulong uuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_metadata_new")]
    private static extern ulong NewMetadata(ulong metadataNewPtr);

    /// <summary>
    /// Retrieves the metadata of a document given its UUID.
    /// </summary>
    /// <param name="uuid">The UUID of the document.</param>
    /// <returns>The metadata of the document.</returns>
    public static Metadata GetApiDocumentMetadata(string uuid)
    {
        var uuidPtr = Pdk.Allocate(uuid).Offset;
        var resultPtr = GetApiDocumentMetadata(uuidPtr);

        return resultPtr.Get<Metadata>();
    }

    /// <summary>
    /// Duplicates a document given its UUID.
    /// </summary>
    /// <param name="uuid">The UUID of the document to duplicate.</param>
    /// <returns>The UUID of the duplicated document.</returns>
    public static string DuplicateDocument(string uuid)
    {
        var uuidPtr = Pdk.Allocate(uuid).Offset;
        var resultPtr = DuplicateDocument(uuidPtr);

        return MemoryBlock.Find(resultPtr).ReadString();
    }

    /// <summary>
    /// Creates a new notebook document.
    /// </summary>
    /// <param name="name">The name of the new notebook.</param>
    /// <param name="parent">The parent document UUID (optional).</param>
    /// <returns>The UUID of the new notebook document.</returns>
    public static string NewNotebook(string name, string? parent = null)
    {
        var notebook = new DocumentNewNotebook()
        {
            Name = name,
            Parent = parent
        };

        var resultPtr = NewNotebook(notebook.GetPointer());

        return MemoryBlock.Find(resultPtr).ReadString();
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
            PdfFile = file
        };

        var resultPtr = NewPdf(notebook.GetPointer());
        return MemoryBlock.Find(resultPtr).ReadString();
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
            EpubFile = file
        };

        var resultPtr = NewEpub(notebook.GetPointer());
        return MemoryBlock.Find(resultPtr).ReadString();
    }

    /// <summary>
    /// Creates a new EPUB document from byte data.
    /// </summary>
    /// <param name="name">The name of the new EPUB document.</param>
    /// <param name="data">The byte data of the EPUB document.</param>
    /// <param name="parent">The parent document UUID (optional).</param>
    /// <returns>The UUID of the new EPUB document.</returns>
    public static string NewEpub(string name, byte[] data, string? parent = null)
    {
        var notebook = new DocumentNewEpub
        {
            Name = name,
            Parent = parent,
            EpubData = data
        };

        var resultPtr = NewEpub(notebook.GetPointer());
        return MemoryBlock.Find(resultPtr).ReadString();
    }

    /// <summary>
    /// This function will modify all the document UUIDs including nested UUID references to a new random UUID and return the new UUID.
    /// </summary>
    /// <param name="uuid">The uuid to randomize</param>
    public static string RandomizeUUIDs(string uuid)
    {
        var ptr = Pdk.Allocate(uuid).Offset;
        var resultPtr = RandomizeUuids(ptr);

        return MemoryBlock.Find(resultPtr).ReadString();
    }

    /// <summary>
    /// Simply unloads any files that Moss has loaded on the document.
    /// Documents will usually be automatically unloaded upon closing.
    /// If your extension loaded files though, this is the way to unload them and you should!
    /// </summary>
    /// <param name="uuid">The uuid to unload</param>
    public static void UnloadFiles(string uuid)
    {
        var ptr = Pdk.Allocate(uuid).Offset;
        UnloadFiles(ptr);
    }

    /// <summary>
    /// Ensures that the document is downloaded.
    /// </summary>
    /// <param name="uuid"></param>
    public static void EnsureDownload(string uuid)
    {
        EnsureDownload(uuid.GetPointer());
    }

    /// <summary>
    /// This function loads all the files enforcing cache usage only. If the cache misses a file, it will not be downloaded.
    /// </summary>
    /// <param name="uuid"></param>
    public static void LoadFilesFromCache(string uuid)
    {
        LoadFilesFromCache(uuid.GetPointer());
    }

    /// <summary>
    /// Create a new metadata object
    /// </summary>
    /// <param name="name">The visible name</param>
    /// <param name="type">Does this metadata corresponds to a notebook or a collection?</param>
    /// <param name="parent">The parent collection</param>
    /// <returns>An id to work with later</returns>
    public static ulong NewMetadata(string name, RMDocumentType type, string? parent = null)
    {
        var md = new MetadataNew
        {
            Name = name,
            DocumentType = type,
            Parent = parent
        };

        return NewMetadata(md.GetPointer());
    }
}