using System.Text.Json.Serialization;
using Moss.NET.Sdk.NEW;
using Moss.NET.Sdk.NEW.Converters;

namespace Moss.NET.Sdk.API;

public class Metadata
{
    [JsonPropertyName("hash")] public string Hash { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(EnumTypeConverter<RMDocumentType>))]
    public RMDocumentType Type { get; set; }

    [JsonPropertyName("parent")] public string Parent { get; set; }

    [JsonPropertyName("created_time")] public long CreatedTime { get; set; }

    [JsonPropertyName("last_modified")] public long LastModified { get; set; }

    [JsonPropertyName("visible_name")] public string VisibleName { get; set; }

    [JsonPropertyName("metadata_modified")]
    public bool MetadataModified { get; set; }

    [JsonPropertyName("modified")] public bool Modified { get; set; }

    [JsonPropertyName("synced")] public bool Synced { get; set; }

    [JsonPropertyName("version")] public int? Version { get; set; }
}