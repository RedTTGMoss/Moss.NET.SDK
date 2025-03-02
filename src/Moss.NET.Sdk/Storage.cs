using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.API;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
public static class Storage
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_metadata_get_all")]
    private static extern ulong GetDocumentMetadata(ulong uuidPtr);

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

    /// <summary>
    /// Retrieves the metadata of a document given its UUID.
    /// </summary>
    /// <param name="uuid">The UUID of the document.</param>
    /// <returns>The metadata of the document.</returns>
    public static Metadata GetDocumentMetadata(string uuid)
    {
        var uuidPtr = Pdk.Allocate(uuid).Offset;
        var resultPtr = GetDocumentMetadata(uuidPtr);

        return Utils.Deserialize(resultPtr, JsonContext.Default.Metadata);
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

        var ptr = Utils.Serialize(notebook, JsonContext.Default.DocumentNewNotebook);
        var resultPtr = NewNotebook(ptr);

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
        var notebook = new DocumentNewPdf()
        {
            Name = name,
            Parent = parent,
            PdfFile = file
        };

        var ptr = Utils.Serialize(notebook, JsonContext.Default.DocumentNewPdf);
        var resultPtr = NewPdf(ptr);

        return MemoryBlock.Find(resultPtr).ReadString();
    }

    /// <summary>
    /// Creates a new PDF document from byte data.
    /// </summary>
    /// <param name="name">The name of the new PDF document.</param>
    /// <param name="data">The byte data of the PDF document.</param>
    /// <param name="parent">The parent document UUID (optional).</param>
    /// <returns>The UUID of the new PDF document.</returns>
    public static string NewPdf(string name, byte[] data, string? parent = null)
    {
        var notebook = new DocumentNewPdf()
        {
            Name = name,
            Parent = parent,
            PdfData = data
        };

        var ptr = Utils.Serialize(notebook, JsonContext.Default.DocumentNewPdf);
        var resultPtr = NewPdf(ptr);

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
        var notebook = new DocumentNewEpub()
        {
            Name = name,
            Parent = parent,
            EpubFile = file
        };

        var ptr = Utils.Serialize(notebook, JsonContext.Default.DocumentNewEpub);
        var resultPtr = NewEpub(ptr);

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
        var notebook = new DocumentNewEpub()
        {
            Name = name,
            Parent = parent,
            EpubData = data
        };

        var ptr = Utils.Serialize(notebook, JsonContext.Default.DocumentNewEpub);
        var resultPtr = NewEpub(ptr);

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
}