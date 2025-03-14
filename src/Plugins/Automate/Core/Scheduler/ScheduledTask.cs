using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NiL.JS.Core;

namespace Automate.Core;

public class ScheduledTask
{
    public DateTime NextRunTime { get; set; }
    public TimeSpan Interval { get; set; }
    public string Name { get; set; }

    public object Data { get; set; }

    [JsonIgnore]
    public ICallable Task { get; set; }

    [JsonIgnore]
    public Predicate<object> Predicate { get; set; }

    public ScheduledTask() { }

    public ScheduledTask(string name, ICallable task, DateTime startTime, TimeSpan interval)
    {
        Name = name;
        Task = task;
        NextRunTime = startTime;
        Interval = interval;
    }

    public void UpdateNextRunTime()
    {
        NextRunTime = NextRunTime.Add(Interval);
    }

    public static string Serialize(List<ScheduledTask> tasks)
    {
        return JsonSerializer.Serialize(tasks, JsonContext.Default.ListScheduledTask);
    }
}