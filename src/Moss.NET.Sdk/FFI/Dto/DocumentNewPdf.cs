using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.FFI.Dto;

internal class DocumentNewPdf
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    [JsonPropertyName("document_uuid")]
    public string? DocumentUuid { get; set; }

    [JsonPropertyName("pdf_data")]
    public byte[]? PdfData { get; set; }

    [JsonPropertyName("pdf_file")]
    public string? PdfFile { get; set; }
}