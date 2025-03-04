﻿using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;
using Moss.NET.Sdk.NEW;

namespace Moss.NET.Sdk.FFI;

public class Accessor
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(SmartEnumNameConverter<AccessorType, int>))]
    public required AccessorType Type { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }
}