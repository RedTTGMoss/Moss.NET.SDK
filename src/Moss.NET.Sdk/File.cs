using System.Text.Json.Serialization;

namespace Moss.NET.Sdk;

public readonly struct File(string key, string path)
{
    [JsonPropertyName("key")] public string Key { get; } = key;

    [JsonPropertyName("path")] public string Path { get; } = path;
}