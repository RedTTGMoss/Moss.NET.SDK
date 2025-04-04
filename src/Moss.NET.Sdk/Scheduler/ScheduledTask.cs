﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Extism;
using Hocon;
using Moss.NET.Sdk.FFI;

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

    [JsonIgnore] public Job Job { get; } = null!;

    [JsonIgnore] public Predicate<object>? Predicate { get; set; }

    public void UpdateNextRunTime()
    {
        NextRunTime = NextRunTime?.Add(Interval);

        Pdk.Log(LogLevel.Error, "next: " + NextRunTime);
    }

    public static string Serialize(List<ScheduledTask> tasks)
    {
        return JsonSerializer.Serialize(tasks, JsonContext.Default.ListScheduledTask);
    }
}