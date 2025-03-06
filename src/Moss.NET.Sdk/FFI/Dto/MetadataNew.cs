using System.Text.Json.Serialization;
using Moss.NET.Sdk.API;
using Moss.NET.Sdk.Core.Converters;
using Moss.NET.Sdk.Storage;

namespace Moss.NET.Sdk.FFI.Dto;

internal class MetadataNew
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    [JsonPropertyName("document_type")]
    [JsonConverter(typeof(EnumTypeConverter<RMDocumentType>))]
    public RMDocumentType? DocumentType { get; set; }
}