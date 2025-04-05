using System.Text.Json.Serialization;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.NEW;

public class RMTag
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("timestamp")] public long Timestamp { get; set; }
}