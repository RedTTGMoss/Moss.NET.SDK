using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.Core.Converters;

public class EnumTypeConverter<T> : JsonConverter<T>
    where T : struct
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Enum.Parse<T>(value!, true);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}