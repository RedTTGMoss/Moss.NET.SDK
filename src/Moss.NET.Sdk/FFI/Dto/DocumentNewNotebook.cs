using System.Text.Json.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.FFI.Dto;

internal class DocumentNewNotebook
{
    [JsonPropertyName("accessor")] public Accessor Accessor { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("parent")] public string? Parent { get; set; }

    [JsonPropertyName("document_uuid")] public string? DocumentUuid { get; set; }

    [JsonPropertyName("page_count")] public int PageCount { get; set; } = 1;

    [JsonPropertyName("notebook_data")] public List<byte[]>? NotebookData { get; set; }

    [JsonPropertyName("notebook_files")] public List<string>? NotebookFiles { get; set; }

    [JsonPropertyName("metadata_id")] public string? MetadataId { get; set; }

    [JsonPropertyName("content_id")] public string? ContentId { get; set; }
}