using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using Totletheyn.DataSources.Weather.Model;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources.Weather;

public class WeatherDataSource : IDataSource
{
    public string Name => "weather";

    private const string ApiUrl =
        "https://api.open-meteo.com/v1/forecast?latitude=49.1113&longitude=9.7391&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m,pressure_msl,precipitation_probability,weathercode&current=temperature_2m,relative_humidity_2m,wind_speed_10m,pressure_msl,precipitation_probability,weathercode&daily=temperature_2m_max,temperature_2m_min,sunrise,sunset,uv_index_max,precipitation_sum,windspeed_10m_max,windgusts_10m_max&forecast_days=3";
    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        var template = new RestTemplate();
        var response = template.GetString(ApiUrl);
        var model = JsonSerializer.Deserialize<WeatherApiResponse>(response);
        
        var items = node.FindNodes("forecastItem").ToArray();

        for (int itemIndex = 0; itemIndex < items.Length; itemIndex++)
        {
            SetWeatherTileData(items, itemIndex, model);
        }
    }

    private void SetWeatherTileData(YogaNode[] items, int itemIndex, WeatherApiResponse? model)
    {
        var itemNode = items[itemIndex];

        itemNode.FindNode<TextNode>("#day")!.Text = $"{itemIndex + 1}. day";

        var weatherCode = GetDominantWeatherCode(model, itemIndex);
        itemNode.FindNode<ImageNode>("#icon")!.Src = WeatherCode.GetImagePath(weatherCode);

        var temperatureMin = model.Daily.Temperature_2m_min[itemIndex];
        var temperatureMax = model.Daily.Temperature_2m_max[itemIndex];

        itemNode.FindNode<TextNode>("temperature max")!.Text = $"{temperatureMax:0.0} {model.Daily_units.Temperature_2m_max}";
        itemNode.FindNode<TextNode>("temperature min")!.Text = $"{temperatureMin:0.0} {model.Daily_units.Temperature_2m_min}";

        double dailyPrecipitationProbability = 0;
        for (int i = 0; i < 24; i++)
        {
            dailyPrecipitationProbability += model.Hourly.Precipitation_probability[itemIndex * 24 + i];
        }

        dailyPrecipitationProbability /= 24;

        itemNode.FindNode<TextNode>("precipitation value")!.Text = $"{dailyPrecipitationProbability:0.0} {model.Hourly_units.Precipitation_probability}";

        var windSpeed = model.Daily.Windspeed_10m_max[itemIndex];
        itemNode.FindNode<TextNode>("wind value")!.Text = $"{windSpeed:0.0} {model.Daily_units.Windspeed_10m_max}";

        var humidity = model.Current.Relative_humidity_2m;
        itemNode.FindNode<TextNode>("humidity value")!.Text = $"{humidity} {model.Current_units.Relative_humidity_2m}";

        var pressure = model.Current.Pressure_msl;
        itemNode.FindNode<TextNode>("pressure value")!.Text = $"{pressure:0.0} {model.Current_units.Pressure_msl}";
    }

    private int GetDominantWeatherCode(WeatherApiResponse model, int dayIndex)
    {
        var startIndex = dayIndex * 24;
        var endIndex = startIndex + 24;

        var weatherCodes = new List<int>();
        for (int i = startIndex; i < endIndex && i < model.Hourly.Weathercode.Count; i++)
        {
            weatherCodes.Add(model.Hourly.Weathercode[i]);
        }

        var groupedWeatherCodes = weatherCodes.GroupBy(x => x);
        var dominantWeatherCode = groupedWeatherCodes.OrderByDescending(x => x.Count()).FirstOrDefault()?.Key ?? 0;

        return dominantWeatherCode;
    }
}