using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Extism;
using Hocon;
using Moss.NET.Sdk.FFI;
using File = System.IO.File;

namespace Moss.NET.Sdk.Scheduler;

public static class TaskScheduler
{
    private static HoconObject _config = null!;
    private static readonly List<ScheduledTask> Tasks = [];
    public static readonly JobActivator Activator = new();

    public static void ScheduleTask(ScheduledTask task)
    {
        task.Name ??= Tasks.Count.ToString();

        Tasks.Add(task);
    }

    internal static void CheckTasks()
    {
        var now = DateTime.UtcNow;

        foreach (var task in Tasks
                     .Where(t => t.NextRunTime <= now)
                     .Where(task => task.Predicate is null || task.Predicate(task))
                     .ToArray())
        {
            Pdk.Log(LogLevel.Info,
                $"Task '{task.Name}': " +
                $"NextRun={task.NextRunTime:O} " +
                $"Interval={task.Interval}");

            task.Task(task.Data!);

            task.UpdateNextRunTime();
        }
    }

    public static void SaveTasks()
    {
        var json = ScheduledTask.Serialize(Tasks);
        File.WriteAllText("extension/.tasks", json);
    }

    public static void Init()
    {
        var json = "[]";
        try
        {
            json = File.ReadAllText("extension/.tasks");
        }
        catch (Exception)
        {
            // ignored
        }

        _config = MossExtension.Config.GetObject("scheduler");

        foreach (var jobInfo in _config.GetObject("jobs"))
        {
            var name = jobInfo.Key;
            var obj = jobInfo.Value.GetObject();

            var interval = obj["interval"].GetString();
            var cls = obj["class"].GetString();
            var options = obj["options"].GetObject();

            var job = Activator.Create(cls);
            job.Interval = GetSpan(interval);
            job.Name = name;
            job.Options = new JobConfig(options);

            Pdk.Log(LogLevel.Debug, $"Scheduling task '{name}' every {interval} using class {cls}");
            ScheduleTask(new ScheduledTask(job.Name, job.Run, DateTime.UtcNow, job.Interval, options));
        }

        AssociateJobInfo(json);
    }

    private static void AssociateJobInfo(string json)
    {
        var taskInfos = JsonSerializer.Deserialize(json, JsonContext.Default.ListScheduledTask);

        if (taskInfos == null) return;

        // Load task info to appropriate job
        foreach (var taskInfo in taskInfos)
        {
            var task = Tasks.FirstOrDefault(t => t.Name == taskInfo.Name);

            if (task is null)
            {
                Pdk.Log(LogLevel.Error, "job not found: " + taskInfo.Name);
                continue;
            }

            task.NextRunTime = taskInfo.NextRunTime ?? DateTimeOffset.UtcNow;

            task.Interval = taskInfo.Interval;
            task.Name = taskInfo.Name;
            task.Data = taskInfo.Data;
        }
    }

    private static TimeSpan GetSpan(string span)
    {
        if (TimeSpan.TryParse(span, out var result))
        {
            return result;
        }

        var unit = span![^1];
        if (int.TryParse(span[..^1], out var value))
        {
            return unit switch
            {
                'd' => TimeSpan.FromDays(value),
                'h' => TimeSpan.FromHours(value),
                'm' => TimeSpan.FromMinutes(value),
                's' => TimeSpan.FromSeconds(value),
                'w' => TimeSpan.FromDays(value * 7),
                'M' => TimeSpan.FromDays(value * 30),
                'y' => TimeSpan.FromDays(value * 365),
                _ => throw new ArgumentOutOfRangeException(nameof(span), span, null)
            };
        }

        return span switch
        {
            "day" => TimeSpan.FromDays(1),
            "hour" => TimeSpan.FromHours(1),
            "minute" => TimeSpan.FromMinutes(1),
            "second" => TimeSpan.FromSeconds(1),
            "week" => TimeSpan.FromDays(7),
            "month" => TimeSpan.FromDays(30),
            "year" => TimeSpan.FromDays(365),
            _ => throw new ArgumentOutOfRangeException(nameof(span), span, null)
        };
    }
}