using System.Collections.Generic;

namespace Totletheyn.DataSources.Weather.Model;

public class WeatherCode
{
    private static readonly Dictionary<int, string> WeatherCodeToImageMap = new()
    {
        { 0, "images/icons/forecast/sunny.png" },
        { 1, "images/icons/forecast/sunny.png" },
        { 2, "images/icons/forecast/cloudy.png" },
        { 3, "images/icons/forecast/cloudy.png" },
        { 45, "images/icons/forecast/fog.png" },
        { 48, "images/icons/forecast/fog.png" },
        { 51, "images/icons/forecast/drizzle.png" },
        { 53, "images/icons/forecast/drizzle.png" },
        { 55, "images/icons/forecast/drizzle.png" },
        { 56, "images/icons/forecast/freezing_drizzle.png" },
        { 57, "images/icons/forecast/freezing_drizzle.png" },
        { 61, "images/icons/forecast/rainy.png" },
        { 63, "images/icons/forecast/rainy.png" },
        { 65, "images/icons/forecast/rainy.png" },
        { 66, "images/icons/forecast/freezing_rain.png" },
        { 67, "images/icons/forecast/freezing_rain.png" },
        { 71, "images/icons/forecast/snow.png" },
        { 73, "images/icons/forecast/snow.png" },
        { 75, "images/icons/forecast/snow.png" },
        { 77, "images/icons/forecast/snow.png" },
        { 80, "images/icons/forecast/showers.png" },
        { 81, "images/icons/forecast/showers.png" },
        { 82, "images/icons/forecast/showers.png" },
        { 85, "images/icons/forecast/snow_showers.png" },
        { 86, "images/icons/forecast/snow_showers.png" },
        { 95, "images/icons/forecast/thunderstorm.png" },
        { 96, "images/icons/forecast/thunderstorm.png" },
        { 99, "images/icons/forecast/thunderstorm.png" }
    };

    public static string GetImagePath(int weatherCode)
    {
        return WeatherCodeToImageMap.GetValueOrDefault(weatherCode, "file://images/icons/forecast/sunny.png");
    }
}