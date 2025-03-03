using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Moss.NET.Sdk.NEW;

namespace Moss.NET.Sdk.Core.Converters;

public class AccessorTypesConverter : JsonConverter<AccessorTypes>
{
    public override void Write(Utf8JsonWriter writer, AccessorTypes value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.GetValue());
    }

    public override AccessorTypes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        return AccessorTypesExtensions.Parse(value!);
    }
}