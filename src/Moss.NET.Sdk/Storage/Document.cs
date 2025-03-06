using Moss.NET.Sdk.Core;

namespace Moss.NET.Sdk.Storage;

public partial class Document : StorageItem
{
    public Document(string name, string? parent = null)
    {
        var uuid = NewNotebook(name, parent);
        Metadata = GetApiDocumentMetadata(uuid);
    }

    private Document()
    {

    }

    public static Document Get(string uuid)
    {
        return new Document { Metadata = GetApiDocumentMetadata(uuid) };
    }

    /// <summary>
    /// This function randomizes a lot of the UUIDs of the document,
    /// it will also update timestamps and set the document to provision.
    /// </summary>
    /// <returns>Returns a new <see cref="Document"/> with the UUID of the new duplicate.</returns>
    public Document Duplicate()
    {
        var newId = DuplicateDocument(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer())
            .ReadString();

        return Get(newId);
    }

    /// <summary>
    /// It will halt until the document is finished downloading.
    /// </summary>
    public void EnsureDownload()
    {
        EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }

    /// <summary>
    /// Calls the callback when the document is finished downloading.
    /// </summary>
    /// <param name="callback"></param>
    public void EnsureDownload(string callback) //Todo: replace callback with Action if dispatching events works properly
    {
        EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer(), callback.GetPointer());
    }

    /// <summary>
    /// Simply unloads any files that Moss has loaded on the document.
    /// Documents will usually be automatically unloaded upon closing.
    /// If your extension loaded files though, this is the way to unload them and you should!
    /// </summary>
    public void UnloadFiles()
    {
        UnloadFiles(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }

    /// <summary>
    /// This will modify all the document UUIDs including nested UUID references to a new random UUID.
    /// </summary>
    public void RandomizeUUIDs()
    {
        var newId = RandomizeUuids(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer()).ReadString();
        Metadata = GetApiDocumentMetadata(newId);
    }

    /// <summary>
    /// This function loads all the files enforcing cache usage only.
    /// If the cache misses a file, it will not be downloaded.
    /// </summary>
    public void LoadFilesFromCache()
    {
        LoadFilesFromCache(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }

    public void Delete(string callback, bool unload = false)
    {
        //StorageFunctions.DeleteNotebook(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer(), callback.GetPointer(), unload ? 1 : 0);
    }

    public void Upload()
    {
        //StorageFunctions.UploadNotebook(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }
}