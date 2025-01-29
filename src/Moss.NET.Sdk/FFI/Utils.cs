using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Extism;

namespace Moss.NET.Sdk.FFI;

internal static class Utils
{
    public static ulong Serialize<T>(T value, JsonTypeInfo<T> typeInfo)
    {
        var data = JsonSerializer.Serialize(value, typeInfo);
        //Pdk.Log(LogLevel.Info, data);

        return Pdk.Allocate(data).Offset;
    }

    public static T Deserialize<T>(ulong ptr, JsonTypeInfo<T> typeInfo)
    {
        var memory = MemoryBlock.Find(ptr);
        var output = memory.ReadString();

        return JsonSerializer.Deserialize(output, typeInfo);
    }

    public static string GetExportName(Action action)
    {
        var attr = action.Method.GetCustomAttribute<UnmanagedCallersOnlyAttribute>();

        if (attr is null)
        {
            throw new InvalidOperationException("The UnmanagedCallersOnlyAttribute is mandatory.");
        }

        return attr.EntryPoint;
    }
}