using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class RmPage
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;

    [JsonPropertyName("index")] public RMTimestampedValue Index { get; set; } = null!;

    [JsonPropertyName("redirect")] public RMTimestampedValue? Redirect { get; set; }

    [JsonPropertyName("scroll_time")] public RMTimestampedValue? ScrollTime { get; set; }

    [JsonPropertyName("vertical_scroll")] public RMTimestampedValue? VerticalScroll { get; set; }
}