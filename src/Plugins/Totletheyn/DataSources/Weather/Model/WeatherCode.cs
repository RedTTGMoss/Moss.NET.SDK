using System.Collections.Generic;

namespace Totletheyn.DataSources.Weather.Model;

public class WeatherCode
{
    private static readonly Dictionary<int, string> WeatherCodeToImageMap = new()
    {
        {0, "file://extension/images/icons/forecast/sunny.png"},
        {1, "file://extension/images/icons/forecast/mostly_sunny.png"},
        {2, "file://extension/images/icons/forecast/partly_cloudy.png"},
        {3, "file://extension/images/icons/forecast/cloudy.png"},
        {45, "file://extension/images/icons/forecast/fog.png"},
        {48, "file://extension/images/icons/forecast/fog.png"},
        {51, "file://extension/images/icons/forecast/drizzle.png"},
        {53, "file://extension/images/icons/forecast/drizzle.png"},
        {55, "file://extension/images/icons/forecast/drizzle.png"},
        {56, "file://extension/images/icons/forecast/freezing_drizzle.png"},
        {57, "file://extension/images/icons/forecast/freezing_drizzle.png"},
        {61, "file://extension/images/icons/forecast/rain.png"},
        {63, "file://extension/images/icons/forecast/rain.png"},
        {65, "file://extension/images/icons/forecast/rain.png"},
        {66, "file://extension/images/icons/forecast/freezing_rain.png"},
        {67, "file://extension/images/icons/forecast/freezing_rain.png"},
        {71, "file://extension/images/icons/forecast/snow.png"},
        {73, "file://extension/images/icons/forecast/snow.png"},
        {75, "file://extension/images/icons/forecast/snow.png"},
        {77, "file://extension/images/icons/forecast/snow.png"},
        {80, "file://extension/images/icons/forecast/showers.png"},
        {81, "file://extension/images/icons/forecast/showers.png"},
        {82, "file://extension/images/icons/forecast/showers.png"},
        {85, "file://extension/images/icons/forecast/snow_showers.png"},
        {86, "file://extension/images/icons/forecast/snow_showers.png"},
        {95, "file://extension/images/icons/forecast/thunderstorm.png"},
        {96, "file://extension/images/icons/forecast/thunderstorm.png"},
        {99, "file://extension/images/icons/forecast/thunderstorm.png"}
    };

    public static string GetImagePath(int weatherCode)
    {
        return WeatherCodeToImageMap.GetValueOrDefault(weatherCode, "file://extension/images/icons/forecast/sunny.png");
    }
}