using System.Text.Json.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.NEW;

public class RMFile
{
    [JsonPropertyName("size")] public long Size { get; set; }

    [JsonPropertyName("rm_filename")] public string RmFilename { get; set; }
}