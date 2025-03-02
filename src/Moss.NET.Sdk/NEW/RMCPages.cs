using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class RMCPages
{
    [JsonPropertyName("pages")] public RmPage[] Pages { get; set; } = null!;

    [JsonPropertyName("original")] public RMTimestampedValue original { get; set; } = null!;

    [JsonPropertyName("last_opened")] public RMTimestampedValue last_opened { get; set; } = null!;

    [JsonPropertyName("uuids")] public RMCPageUUID[] uuids { get; set; } = null!;
}