using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Extism;

namespace Moss.NET.Sdk.FFI;

internal static class Utils
{
    public static ulong Serialize<T>(T value, JsonTypeInfo<T> typeInfo)
    {
        var data = JsonSerializer.Serialize(value, typeInfo);

#if DEBUG
        if (data.Length < 150)
        {
            Pdk.Log(LogLevel.Info, $"{typeof(T).FullName}:{data}");
        }
#endif

        return Pdk.Allocate(data).Offset;
    }

    public static T? Deserialize<T>(ulong ptr, JsonTypeInfo<T> typeInfo)
    {
        var memory = MemoryBlock.Find(ptr);
        var output = memory.ReadString();

        if (string.IsNullOrEmpty(output))
        {
            Pdk.Log(LogLevel.Error, $"Deserialization failed: empty string for {typeInfo.Type.Name}");
            return default;
        }

#if DEBUG
        Pdk.Log(LogLevel.Info, $"{typeInfo.Type.Name}: {output}");
#endif

        try
        {
            return JsonSerializer.Deserialize(output, typeInfo)!;
        }
        catch (Exception e)
        {
            Pdk.Log(LogLevel.Error, e.Message + $"({typeInfo.Type.Name}): " + output);
            return default;
        }
    }
}