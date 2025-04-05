using System.Text.Json.Serialization;

namespace Moss.NET.Sdk;

public class RootInfo
{
    [JsonPropertyName("generation")] public ulong Generation { get; set; }

    [JsonPropertyName("hash")] public string Hash { get; set; } = null!;
}