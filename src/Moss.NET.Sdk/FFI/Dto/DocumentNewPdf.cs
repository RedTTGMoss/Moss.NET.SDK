using System.Text.Json.Serialization;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Core.Converters;

namespace Moss.NET.Sdk.FFI.Dto;

internal class DocumentNewPdf
{
    [JsonPropertyName("accessor")]
    public Accessor Accessor { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    [JsonPropertyName("document_uuid")]
    public string? DocumentUuid { get; set; }

    [JsonPropertyName("pdf_file")]
    public string? PdfFile { get; set; }

    [JsonPropertyName("pdf_data")]
    [JsonConverter(typeof(Base64Converter))]
    public Base64 PdfData { get; set; }
}