using Ardalis.SmartEnum;

namespace Moss.NET.Sdk;

// using with:
// [JsonConverter(typeof(SmartEnumNameConverter<AccessorType, int>))]
public sealed class AccessorType : SmartEnum<AccessorType>
{
    public static readonly AccessorType APIDocumentMetadata = new("api_document_metadata", 1);
    public static readonly AccessorType APIDocumentContent = new("api_document_content", 2);

    public static readonly AccessorType APICollectionMetadata = new("api_collection_metadata", 3);

    public static readonly AccessorType APIDocument = new( "api_document", 4);
    public static readonly AccessorType APICollection = new("api_collection", 5);

    public static readonly AccessorType StandaloneDocumentMetadata = new("document_metadata", 6);
    public static readonly AccessorType StandaloneDocumentContent = new("document_content", 7);

    public static readonly AccessorType StandaloneCollectionMetadata = new("collection_metadata", 8);

    public static readonly AccessorType StandaloneDocument = new("document", 9);
    public static readonly AccessorType StandaloneCollection = new("collection", 10);

    public static readonly AccessorType StandaloneMetadata = new("metadata", 11);
    public static readonly AccessorType StandaloneContent = new("content", 12);

    public static readonly AccessorType FileSyncProgress = new("file_sync_progress", 13);
    public static readonly AccessorType DocumentSyncProgress = new("document_sync_progress", 14);

    private AccessorType(string name, int value) : base(name, value)
    {
    }

    public static implicit operator string(AccessorType accessorType) => accessorType.Name;
}
