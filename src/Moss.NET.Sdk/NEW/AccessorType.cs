using System;

namespace Moss.NET.Sdk.NEW;

public enum AccessorTypes
{
    // Document API SUB
    APIDocumentMetadata,
    APIDocumentContent,

    // Collection API SUB
    APICollectionMetadata,

    // API
    APIDocument,
    APICollection,

    // Document Standalone SUB
    StandaloneDocumentMetadata,
    StandaloneDocumentContent,

    // Collection Standalone SUB
    StandaloneCollectionMetadata,

    // Standalone
    StandaloneDocument,
    StandaloneCollection,

    StandaloneMetadata,
    StandaloneContent
}

public static class AccessorTypesExtensions
{
    private static readonly string[] Values =
    [
        "api_document_metadata",
        "api_document_content",
        "api_collection_metadata",
        "api_document",
        "api_collection",
        "document_metadata",
        "document_content",
        "collection_metadata",
        "document",
        "collection",
        "metadata",
        "content"
    ];

    public static string GetValue(this AccessorTypes accessorType)
    {
        return Values[(int)accessorType];
    }

    public static AccessorTypes Parse(string value)
    {
        var index = Array.IndexOf(Values, value);
        if (index != -1)
        {
            return (AccessorTypes)index;
        }

        throw new ArgumentException($"Invalid AccessorTypes value: {value}");
    }
}