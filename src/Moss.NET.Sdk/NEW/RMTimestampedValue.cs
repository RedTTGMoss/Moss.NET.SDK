namespace Moss.NET.Sdk.NEW;

using System.Text.Json.Serialization;

public class RMTimestampedValue
{
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }

    [JsonPropertyName("value")]
    public object Value { get; set; }
}
