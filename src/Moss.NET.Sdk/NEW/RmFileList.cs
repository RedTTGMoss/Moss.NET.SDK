using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class RmFileList
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("files")]
    public RMFile[] Files { get; set; }
}