using System.Text.Json.Serialization;
using Totletheyn.DataSources.Weather.Model;

namespace Totletheyn.DataSources.Weather;

[JsonSerializable(typeof(WeatherApiResponse))]
[JsonSerializable(typeof(Current))]
[JsonSerializable(typeof(CurrentUnits))]
[JsonSerializable(typeof(Daily))]
[JsonSerializable(typeof(DailyUnits))]
[JsonSerializable(typeof(Hourly))]
[JsonSerializable(typeof(HourlyUnits))]
[JsonSerializable(typeof(WeatherCode))]
public partial class WeatherJsonContext : JsonSerializerContext
{
}