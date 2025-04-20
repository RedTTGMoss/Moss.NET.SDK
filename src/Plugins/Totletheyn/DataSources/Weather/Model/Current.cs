using System.Text.Json.Serialization;

namespace Totletheyn.DataSources.Weather.Model;

public class Current
{
    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("interval")]
    public int Interval { get; set; }

    [JsonPropertyName("temperature_2m")]
    public double Temperature_2m { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public int Relative_humidity_2m { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double Wind_speed_10m { get; set; }

    [JsonPropertyName("pressure_msl")]
    public double Pressure_msl { get; set; }

    [JsonPropertyName("precipitation_probability")]
    public int Precipitation_probability { get; set; }

    [JsonPropertyName("weathercode")]
    public int Weathercode { get; set; }
}