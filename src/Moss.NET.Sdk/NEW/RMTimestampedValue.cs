using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class RMTimestampedValue
{
    [JsonPropertyName("timestamp")] public string Timestamp { get; set; }

    [JsonPropertyName("value")] public object Value { get; set; }
}