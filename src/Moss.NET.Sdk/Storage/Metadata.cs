using System.Text.Json.Serialization;
using Moss.NET.Sdk.API;
using Moss.NET.Sdk.Core.Converters;

namespace Moss.NET.Sdk.Storage;

public class Metadata
{
    [JsonPropertyName("hash")] public string Hash { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(EnumTypeConverter<RMDocumentType>))]
    public RMDocumentType Type { get; set; }

    [JsonPropertyName("accessor")]
    public Accessor Accessor { get; set; }

    [JsonPropertyName("parent")] public string? Parent { get; set; }

    [JsonPropertyName("created_time")] public long CreatedTime { get; set; }

    [JsonPropertyName("last_modified")] public long LastModified { get; set; }

    [JsonPropertyName("visible_name")] public string VisibleName { get; set; }

    [JsonPropertyName("metadata_modified")]
    public bool MetadataModified { get; set; }

    [JsonPropertyName("modified")] public bool Modified { get; set; }

    [JsonPropertyName("synced")] public bool Synced { get; set; }

    [JsonPropertyName("version")] public int? Version { get; set; }
}