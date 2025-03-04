using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.FFI.Dto;

internal class DocumentNewEpub
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    [JsonPropertyName("document_uuid")]
    public string? DocumentUuid { get; set; }

    [JsonPropertyName("epub_data")]
    public byte[]? EpubData { get; set; }

    [JsonPropertyName("epub_file")]
    public string? EpubFile { get; set; }

    [JsonPropertyName("accessor")]
    public Accessor Accessor { get; set; }
}