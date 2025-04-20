using System.Text.Json.Serialization;

namespace Totletheyn.DataSources.Weather.Model;

public class CurrentUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("interval")]
    public string Interval { get; set; }

    [JsonPropertyName("temperature_2m")]
    public string Temperature_2m { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public string Relative_humidity_2m { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public string Wind_speed_10m { get; set; }

    [JsonPropertyName("pressure_msl")]
    public string Pressure_msl { get; set; }

    [JsonPropertyName("precipitation_probability")]
    public string Precipitation_probability { get; set; }

    [JsonPropertyName("weathercode")]
    public string Weathercode { get; set; }
}