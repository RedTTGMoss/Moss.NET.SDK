using System.Text.Json.Serialization;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.NEW;

public class RmFileList
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("files")]
    public RMFile[] Files { get; set; }
}