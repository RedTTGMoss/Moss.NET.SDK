using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;

namespace Moss.NET.Sdk;

public class Accessor
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(SmartEnumNameConverter<AccessorType, int>))]
    public required AccessorType Type { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("id")]
    public ulong? Id { get; set; }
}