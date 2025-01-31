namespace Moss.NET.Sdk.NEW;

using System.Text.Json.Serialization;

public class RmPage
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("index")]
    public RMTimestampedValue Index { get; set; }

    [JsonPropertyName("redirect")]
    public RMTimestampedValue? Redirect { get; set; }

    [JsonPropertyName("scroll_time")]
    public RMTimestampedValue? ScrollTime { get; set; }

    [JsonPropertyName("vertical_scroll")]
    public RMTimestampedValue? VerticalScroll { get; set; }
}