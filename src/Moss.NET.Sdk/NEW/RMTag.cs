using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class RMTag
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("timestamp")] public long Timestamp { get; set; }
}