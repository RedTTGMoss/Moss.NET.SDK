namespace Moss.NET.Sdk.NEW;

using System.Text.Json.Serialization;

public class RMCPageUUID
{
    [JsonPropertyName("first")]
    public string First { get; set; }

    [JsonPropertyName("second")]
    public long Second { get; set; }
}
