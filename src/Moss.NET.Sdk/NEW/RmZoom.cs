using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.NEW;

public class RmZoom
{
    [JsonPropertyName("zoomMode")] public RMZoomMode ZoomMode { get; set; }

    [JsonPropertyName("customZoomCenterX")]
    public long CustomZoomCenterX { get; set; }

    [JsonPropertyName("customZoomCenterY")]
    public long CustomZoomCenterY { get; set; }

    [JsonPropertyName("customZoomPageHeight")]
    public long CustomZoomPageHeight { get; set; }

    [JsonPropertyName("customZoomPageWidth")]
    public long CustomZoomPageWidth { get; set; }

    [JsonPropertyName("customZoomScale")] public long CustomZoomScale { get; set; }
}