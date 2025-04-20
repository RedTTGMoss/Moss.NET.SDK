using System.Text.Json.Serialization;
using Extism;
using Hocon;
using LiteDB;

namespace Moss.NET.Sdk.Scheduler;

public class ScheduledTask
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
    [JsonIgnore] public TimeSpan Interval { get; set; }

    [JsonIgnore] public HoconObject Options { get; set; } = null!;

    public string? Name { get; set; }

    public object? Data { get; set; } = new();

    [BsonIgnore] public Job Job { get; } = null!;

    [BsonIgnore] public Predicate<object>? Predicate { get; set; }

    public ObjectId _id { get; set; }

    public void UpdateNextRunTime()
    {
        NextRunTime = NextRunTime?.Add(Interval);

        Pdk.Log(LogLevel.Error, "next: " + NextRunTime);
    }
}