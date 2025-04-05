using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class FileSyncProgress
{
    [JsonPropertyName("done")] public ulong Done { get; set; }

    [JsonPropertyName("total")] public ulong Total { get; set; }

    [JsonPropertyName("stage")] public string? Stage { get; set; }

    [JsonPropertyName("finished")] public bool Finished { get; set; }

    [JsonPropertyName("accessor")] public Accessor Accessor { get; set; }
}