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

    public Document Duplicate()
    {
        var newId = DuplicateDocument(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer())
            .ReadString();

        return Get(newId);
    }

    public void EnsureDownload()
    {
        EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }

    public void EnsureDownload(string callback) //Todo: replace callback with Action if dispatching events works properly
    {
        EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer(), callback.GetPointer());
    }
}