using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Totletheyn.DataSources.Weather.Model;

public class Hourly
{
    [JsonPropertyName("time")] public List<string> Time { get; set; }

    [JsonPropertyName("temperature_2m")] public List<double> Temperature_2m { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public List<int> Relative_humidity_2m { get; set; }

    [JsonPropertyName("wind_speed_10m")] public List<double> Wind_speed_10m { get; set; }

    [JsonPropertyName("pressure_msl")] public List<double> Pressure_msl { get; set; }

    [JsonPropertyName("precipitation_probability")]
    public List<int> Precipitation_probability { get; set; }

    [JsonPropertyName("weathercode")] public List<int> Weathercode { get; set; }
}