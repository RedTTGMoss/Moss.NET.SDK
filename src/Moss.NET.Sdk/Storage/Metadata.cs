using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Core.Converters;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.Storage;

public class Metadata
{
    [JsonPropertyName("hash")] public string Hash { get; init;}

    [JsonPropertyName("type")]
    [JsonConverter(typeof(EnumTypeConverter<RMDocumentType>))]
    public RMDocumentType Type { get; init; }

    [JsonPropertyName("accessor")]
    public Accessor Accessor { get; init;}

    [JsonPropertyName("parent")] public string? Parent { get; init; }

    [JsonPropertyName("created_time")] public long CreatedTime { get; init; }

    [JsonPropertyName("last_modified")] public long LastModified { get; init; }

    [JsonPropertyName("visible_name")] public string VisibleName { get; init; }

    [JsonPropertyName("metadata_modified")]
    public bool MetadataModified { get; init; }

    [JsonPropertyName("modified")] public bool Modified { get; init; }

    [JsonPropertyName("synced")] public bool Synced { get; init; }

    [JsonPropertyName("version")] public int? Version { get; init; }

    [DllImport(Functions.DLL, EntryPoint = "_moss_api_set")]
    private static extern void Set(ulong accessorPtr, ulong configSetPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_get_all")]
    private static extern ulong Get(ulong accessorPtr); // -> ConfigGet[T]

    [DllImport(Functions.DLL, EntryPoint = "moss_api_get")]
    private static extern ulong Get(ulong accessorPtr, ulong keyPtr); // -> ConfigGet[T]

    public void Set(string key, object value)
    {
        Set(new Accessor
        {
            Type = AccessorType.APIDocumentMetadata,
            Uuid = Accessor.Uuid
        }, key, value);
    }

    public void Set(Accessor accessor, string key, object value)
    {
        Set(accessor.GetPointer(),new ConfigSet(key, value).GetPointer());
    }

    public T Get<T>(string key)
    {
        var accessor = new Accessor
        {
            Type = AccessorType.APIDocumentMetadata,
            Uuid = Accessor.Uuid
        };

        return Get<T>(accessor, key);
    }

    public static T Get<T>(Accessor accessor, string key)
    {
        var resultPtr = Get(accessor.GetPointer(), key.GetPointer());
        var result = resultPtr.Get<ConfigGetD>();

        return result.value.GetValue<T>();
    }

    public static T Get<T>(Accessor accessor)
    {
        var resultPtr = Get(accessor.GetPointer());
        var result = resultPtr.Get<ConfigGetD>();

        return result.value.GetValue<T>();
    }

    public static Metadata Get(string uuid)
    {
        var accessor = new Accessor()
        {
            Type = AccessorType.APIDocumentMetadata,
            Uuid = uuid
        };

        return Get<Metadata>(accessor);
    }
}