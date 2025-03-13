using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Automate.Core;

public class ScheduledTask
{
    public string Id { get; set; }
    public string TaskName { get; set; }
    public DateTime NextRunTime { get; set; }
    public TimeSpan Interval { get; set; }

    [JsonIgnore]
    public Action Task { get; set; }

    public ScheduledTask() { }

    public ScheduledTask(string id, string taskName, Action task, DateTime startTime, TimeSpan interval)
    {
        Id = id;
        TaskName = taskName;
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
        return JsonSerializer.Serialize(tasks);
    }

    public static List<ScheduledTask> Deserialize(string json)
    {
        return JsonSerializer.Deserialize<List<ScheduledTask>>(json);
    }
}