using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Core.Converters;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.Storage;

public class Metadata
{
    [JsonPropertyName("hash")] public string Hash { get; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(EnumTypeConverter<RMDocumentType>))]
    public RMDocumentType Type { get; }

    [JsonPropertyName("accessor")]
    public Accessor Accessor { get; }

    [JsonPropertyName("parent")] public string? Parent { get; }

    [JsonPropertyName("created_time")] public long CreatedTime { get; }

    [JsonPropertyName("last_modified")] public long LastModified { get; }

    [JsonPropertyName("visible_name")] public string VisibleName { get; }

    [JsonPropertyName("metadata_modified")]
    public bool MetadataModified { get; }

    [JsonPropertyName("modified")] public bool Modified { get; }

    [JsonPropertyName("synced")] public bool Synced { get; }

    [JsonPropertyName("version")] public int? Version { get; }

    [DllImport(Functions.DLL, EntryPoint = "_moss_api_set")]
    private static extern void Set(ulong accessorPtr, ulong configSetPtr);

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
}