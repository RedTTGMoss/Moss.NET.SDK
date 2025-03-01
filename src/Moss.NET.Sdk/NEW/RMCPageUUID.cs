using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class RMCPageUUID
{
    [JsonPropertyName("first")] public string First { get; set; }

    [JsonPropertyName("second")] public long Second { get; set; }
}