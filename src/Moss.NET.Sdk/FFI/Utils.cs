using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Extism;

namespace Moss.NET.Sdk;

internal static class Utils
{
    public static ulong Serialize<T>(T value, JsonTypeInfo<T> typeInfo)
    {
        return Pdk.Allocate(JsonSerializer.Serialize(value, typeInfo)).Offset;
    }

    public static T Deserialize<T>(ulong ptr, JsonTypeInfo<T> typeInfo)
    {
        var memory = MemoryBlock.Find(ptr);
        var output = memory.ReadString();

        return JsonSerializer.Deserialize(output, typeInfo);
    }
}