using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Totletheyn.DataSources.Weather.Model;

public class Daily
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; }

    [JsonPropertyName("temperature_2m_max")]
    public List<double> Temperature_2m_max { get; set; }

    [JsonPropertyName("temperature_2m_min")]
    public List<double> Temperature_2m_min { get; set; }

    [JsonPropertyName("sunrise")]
    public List<string> Sunrise { get; set; }

    [JsonPropertyName("sunset")]
    public List<string> Sunset { get; set; }

    [JsonPropertyName("uv_index_max")]
    public List<double> Uv_index_max { get; set; }

    [JsonPropertyName("precipitation_sum")]
    public List<double> Precipitation_sum { get; set; }

    [JsonPropertyName("windspeed_10m_max")]
    public List<double> Windspeed_10m_max { get; set; }

    [JsonPropertyName("windgusts_10m_max")]
    public List<double> Windgusts_10m_max { get; set; }
}