using System.Text.Json;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.Core.Converters;

internal class Base64Converter : JsonConverter<Base64>
{
    public override Base64? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return new Base64(value!);
    }

    public override void Write(Utf8JsonWriter writer, Base64 value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}