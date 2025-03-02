using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.FFI;

internal class DocumentNewNotebook
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    [JsonPropertyName("document_uuid")]
    public string? DocumentUuid { get; set; }

    [JsonPropertyName("page_count")] public int PageCount { get; set; } = 1;

    [JsonPropertyName("notebook_data")]
    public List<byte[]>? NotebookData { get; set; }

    [JsonPropertyName("notebook_files")]
    public List<string>? NotebookFiles { get; set; }

    [JsonPropertyName("metadata_id")]
    public string? MetadataId { get; set; }

    [JsonPropertyName("content_id")]
    public string? ContentId { get; set; }
}