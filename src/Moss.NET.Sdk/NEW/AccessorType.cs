using Ardalis.SmartEnum;

namespace Moss.NET.Sdk.NEW;

// using with:
// [JsonConverter(typeof(SmartEnumNameConverter<TestEnum,int>))]
public sealed class AccessorTypes : SmartEnum<AccessorTypes>
{
    public static readonly AccessorTypes APIDocumentMetadata = new("api_document_metadata", 1);
    public static readonly AccessorTypes APIDocumentContent = new("api_document_content", 2);

    public static readonly AccessorTypes APICollectionMetadata = new("api_collection_metadata", 3);

    public static readonly AccessorTypes APIDocument = new( "api_document", 4);
    public static readonly AccessorTypes APICollection = new("api_collection", 5);

    public static readonly AccessorTypes StandaloneDocumentMetadata = new("document_metadata", 6);
    public static readonly AccessorTypes StandaloneDocumentContent = new("document_content", 7);

    public static readonly AccessorTypes StandaloneCollectionMetadata = new("collection_metadata", 8);

    public static readonly AccessorTypes StandaloneDocument = new("document", 9);
    public static readonly AccessorTypes StandaloneCollection = new("collection", 10);

    public static readonly AccessorTypes StandaloneMetadata = new("metadata", 11);
    public static readonly AccessorTypes StandaloneContent = new("content", 12);

    private AccessorTypes(string name, int value) : base(name, value)
    {
    }

    public static implicit operator string(AccessorTypes accessorType) => accessorType.Name;
}
