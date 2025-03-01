namespace Moss.NET.Sdk.NEW;

using System.Text.Json.Serialization;

public class RMFile
{
    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("rm_filename")]
    public string RmFilename { get; set; }
}