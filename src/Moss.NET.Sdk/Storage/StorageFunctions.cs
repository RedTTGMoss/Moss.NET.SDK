using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.Storage;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
public static class StorageFunctions
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_metadata_new")]
    private static extern ulong NewMetadata(ulong metadataNewPtr);

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

    public static ulong UploadManyDocuments(IEnumerable<Accessor> accessors, string callback, bool unload = false)
    {
        return UploadManyDocuments(accessors.ToArray().GetPointer(), callback.GetPointer(), unload ? 1 : 0);
    }

    public static ulong DeleteManyDocuments(IEnumerable<Accessor> accessors, string callback, bool unload = false)
    {
        return DeleteManyDocuments(accessors.ToArray().GetPointer(), callback.GetPointer(), unload ? 1 : 0);
    }

    public static void UploadManyDocuments(IEnumerable<Document> documents, Action? callback = null,
        bool unload = false)
    {
        var accessors = documents.Select(d => new Accessor
        {
            Type = AccessorType.APIDocument,
            Uuid = d.Metadata.Accessor.Uuid
        });

        var taskid = UploadManyDocuments(accessors, "dispatch_entry", unload);
        Dispatcher.Register(taskid, callback);
    }

    public static void DeleteManyDocuments(IEnumerable<Document> documents, Action? callback = null,
        bool unload = false)
    {
        var accessors = documents.Select(d => new Accessor
        {
            Type = AccessorType.APIDocument,
            Uuid = d.Metadata.Accessor.Uuid
        });

        var taskid = DeleteManyDocuments(accessors, "dispatch_entry", unload);
        Dispatcher.Register(taskid, callback);
    }

    /// <summary>
    ///     Create a new metadata object
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
}