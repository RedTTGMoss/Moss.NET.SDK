using System.Text.Json.Serialization;

namespace Totletheyn.DataSources.Weather.Model;

public class DailyUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("temperature_2m_max")]
    public string Temperature_2m_max { get; set; }

    [JsonPropertyName("temperature_2m_min")]
    public string Temperature_2m_min { get; set; }

    [JsonPropertyName("sunrise")]
    public string Sunrise { get; set; }

    [JsonPropertyName("sunset")]
    public string Sunset { get; set; }

    [JsonPropertyName("uv_index_max")]
    public string Uv_index_max { get; set; }

    [JsonPropertyName("precipitation_sum")]
    public string Precipitation_sum { get; set; }

    [JsonPropertyName("windspeed_10m_max")]
    public string Windspeed_10m_max { get; set; }

    [JsonPropertyName("windgusts_10m_max")]
    public string Windgusts_10m_max { get; set; }
}