using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Extism;
using Moss.NET.Sdk.Core.Converters;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.Scheduler;

public class ScheduledTask
{
    public ScheduledTask()
    {
    }

    public ScheduledTask(string? name, Action<object?>? task, DateTimeOffset? startTime, TimeSpan interval)
    {
        Name = name;
        Task = task;
        NextRunTime = startTime;
        Interval = interval;
    }

    public DateTimeOffset? NextRunTime { get; set; }
    public TimeSpan Interval { get; set; }
    public string? Name { get; set; }

    public object? Data { get; set; }

    [JsonIgnore] public Action<object?>? Task { get; set; }

    [JsonIgnore] public Predicate<object>? Predicate { get; set; }

    public void UpdateNextRunTime()
    {
        NextRunTime = NextRunTime?.Add(Interval);

        Pdk.Log(LogLevel.Error, "next: "+ NextRunTime);
    }

    public static string Serialize(List<ScheduledTask> tasks)
    {
        return JsonSerializer.Serialize(tasks, JsonContext.Default.ListScheduledTask);
    }
}