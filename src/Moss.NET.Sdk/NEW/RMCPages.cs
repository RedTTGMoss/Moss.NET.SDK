namespace Moss.NET.Sdk.NEW;

using System.Text.Json.Serialization;

public class RMCPages
{
    [JsonPropertyName("pages")]
    public RmPage[] Pages { get; set; }

    [JsonPropertyName("original")]
    public RMTimestampedValue original { get; set; }

    [JsonPropertyName("last_opened")]
    public RMTimestampedValue last_opened { get; set; }

    [JsonPropertyName("uuids")]
    public RMCPageUUID[] uuids { get; set; }
}
