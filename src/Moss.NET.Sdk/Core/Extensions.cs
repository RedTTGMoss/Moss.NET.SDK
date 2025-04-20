using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Extism;
using Hocon;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.Core;

public static class Extensions
{
    public static T GetValue<T>(this JsonElement element)
    {
        return (element.ValueKind switch
        {
            JsonValueKind.String => (T?)(object?)element.GetString(),
            JsonValueKind.Number when typeof(T) == typeof(int) => (T)(object)element.GetInt32(),
            JsonValueKind.Number when typeof(T) == typeof(long) => (T)(object)element.GetInt64(),
            JsonValueKind.Number when typeof(T) == typeof(double) => (T)(object)element.GetDouble(),
            JsonValueKind.Number when typeof(T) == typeof(decimal) => (T)(object)element.GetDecimal(),
            JsonValueKind.True or JsonValueKind.False => (T)(object)element.GetBoolean(),
            JsonValueKind.Null => default,
            JsonValueKind.Object => JsonSerializer.Deserialize(element.GetRawText(),
                ((JsonTypeInfo<T>)JsonContext.Default.GetTypeInfo(typeof(T))!)!),
            _ => throw new InvalidOperationException($"Unsupported value kind: {element.ValueKind}")
        })!;
    }

    public static ulong GetPointer(this string str)
    {
        return Pdk.Allocate(str).Offset;
    }

    public static ulong GetPointer<T>(this T @this)
    {
        return GetPointer(@this, (JsonTypeInfo<T>)JsonContext.Default.GetTypeInfo(typeof(T))!);
    }

    public static ulong GetPointer<T>(this T @this, JsonTypeInfo<T> typeInfo)
    {
        return Utils.Serialize(@this, typeInfo);
    }

    public static T? Get<T>(this ulong ptr, JsonTypeInfo<T> typeInfo)
    {
        return Utils.Deserialize(ptr, typeInfo);
    }

    public static T Get<T>(this ulong ptr)
    {
        return Get(ptr, (JsonTypeInfo<T>)JsonContext.Default.GetTypeInfo(typeof(T))!)!;
    }

    public static MemoryBlock Find(this ulong ptr)
    {
        return MemoryBlock.Find(ptr);
    }

    public static string ReadString(this ulong ptr)
    {
        return MemoryBlock.Find(ptr).ReadString();
    }

    public static dynamic Get(this HoconRoot root, string path)
    {
        return new Options(root.GetObject(path));
    }

    public static bool GetBoolean(this HoconField root)
    {
        return root.Value.GetBoolean();
    }

    public static void Measure(string name, Action method)
    {
        var meter = MossExtension.Instance!.Meter;
        if (meter is null)
        {
            method();
            return;
        }

        var methodDuration = meter.CreateHistogram<double>(name + "_duration", "ms", "Method duration in milliseconds");

        var stopwatch = Stopwatch.StartNew();
        method();
        stopwatch.Stop();
        methodDuration.Record(stopwatch.Elapsed.TotalMilliseconds);
    }
}