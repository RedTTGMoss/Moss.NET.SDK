using System.Text.Json.Serialization;
using Moss.NET.Sdk.API;

namespace Moss.NET.Sdk.FFI.Dto;

public class MetadataNew
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    [JsonPropertyName("document_type")]
    public RMDocumentType? DocumentType { get; set; }
}