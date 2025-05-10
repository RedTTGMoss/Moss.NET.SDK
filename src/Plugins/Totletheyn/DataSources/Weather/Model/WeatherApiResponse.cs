using System.Text.Json.Serialization;

namespace Totletheyn.DataSources.Weather.Model;

public class WeatherApiResponse
{
    [JsonPropertyName("latitude")] public double Latitude { get; set; }

    [JsonPropertyName("longitude")] public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double Generationtime_ms { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int Utc_offset_seconds { get; set; }

    [JsonPropertyName("timezone")] public string Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public string Timezone_abbreviation { get; set; }

    [JsonPropertyName("elevation")] public double Elevation { get; set; }

    [JsonPropertyName("current_units")] public CurrentUnits Current_units { get; set; }

    [JsonPropertyName("current")] public Current Current { get; set; }

    [JsonPropertyName("hourly_units")] public HourlyUnits Hourly_units { get; set; }

    [JsonPropertyName("hourly")] public Hourly Hourly { get; set; }

    [JsonPropertyName("daily_units")] public DailyUnits Daily_units { get; set; }

    [JsonPropertyName("daily")] public Daily Daily { get; set; }
}