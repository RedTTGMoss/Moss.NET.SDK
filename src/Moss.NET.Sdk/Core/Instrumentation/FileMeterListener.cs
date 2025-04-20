using System.Diagnostics.Metrics;

namespace Moss.NET.Sdk.Core.Instrumentation;

internal class FileMeterListener : MeterListener<double>
{
    private readonly string _filePath;

    public FileMeterListener(string filePath)
    {
        _filePath = filePath;

        if (!File.Exists(_filePath)) File.Create(filePath);
    }

    protected override void OnMeasurementRecorded(Instrument instrument, double measurement,
        ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
    {
        var tagList = new List<string>();
        foreach (var tag in tags) tagList.Add($"{tag.Key}: {tag.Value}");

        using var writer = new StreamWriter(_filePath, true);
        writer.WriteLine(
            $"{DateTime.UtcNow:O} - {instrument.Name}, Measurement: {measurement}{instrument.Unit}, Tags: {string.Join(", ", tagList)}");
    }
}