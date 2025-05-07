using System.Text.Json.Serialization;
using Extism;
using Hocon;
using PolyType;

namespace Moss.NET.Sdk.Scheduler;

[GenerateShape]
public partial class ScheduledTask
{
    public ScheduledTask()
    {
    }

    public ScheduledTask(string? name, Job job, DateTimeOffset? startTime, TimeSpan interval, HoconObject options)
    {
        Name = name;
        Job = job;
        NextRunTime = startTime;
        Interval = interval;
        Options = options;
    }

    public DateTimeOffset? NextRunTime { get; set; }
    [PropertyShape(Ignore = true)] public TimeSpan Interval { get; set; }

    [PropertyShape(Ignore = true)] public HoconObject Options { get; set; } = null!;

    public string? Name { get; set; }

    [PropertyShape(Ignore = true)]
    public object? Data { get; set; } = new();

    [PropertyShape(Ignore = true)] public Job Job { get; } = null!;

    [PropertyShape(Ignore = true)] public Predicate<object>? Predicate { get; set; }

    public void UpdateNextRunTime()
    {
        NextRunTime = NextRunTime?.Add(Interval);

        Pdk.Log(LogLevel.Error, "next: " + NextRunTime);
    }
}