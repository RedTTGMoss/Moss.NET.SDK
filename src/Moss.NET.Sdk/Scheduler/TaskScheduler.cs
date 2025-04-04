using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Extism;
using Hocon;
using Moss.NET.Sdk.FFI;
using File = System.IO.File;

namespace Moss.NET.Sdk.Scheduler;

public static class TaskScheduler
{
    private static HoconObject _config = null!;
    private static readonly List<ScheduledTask> Jobs = [];
    public static readonly JobActivator Activator = new();

    public static void ScheduleTask(ScheduledTask task)
    {
        task.Name ??= Jobs.Count.ToString();

        Jobs.Add(task);
    }

    internal static void RunJobs()
    {
        var now = DateTime.UtcNow;

        foreach (var task in Jobs
                     .Where(t => t.NextRunTime <= now)
                     .Where(task => task.Predicate is null || task.Predicate(task))
                     .ToArray())
        {
            Pdk.Log(LogLevel.Info,
                $"Task '{task.Name}': " +
                $"NextRun={task.NextRunTime:O} " +
                $"Interval={task.Interval}");

            task.Job.Data = task.Data!;
            task.Job.Run();

            task.UpdateNextRunTime();
        }
    }

    public static void SaveTasks()
    {
        foreach (var job in Jobs)
        {
            job.Data = job.Job.Data;
            job.Job.Shutdown();
        }

        var json = ScheduledTask.Serialize(Jobs);
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

        ReadJobConfig(json);
    }

    private static void ReadJobConfig(string json)
    {
        if (MossExtension.Config is null)
        {
            return;
        }

        _config = MossExtension.Config.GetObject("scheduler");

        foreach (var jobInfo in _config.GetObject("jobs"))
        {
            var cls = jobInfo.Key;
            var obj = jobInfo.Value.GetObject();

            var interval = obj["interval"].GetString();
            var name = obj["name"].GetString();
            var options = obj["options"].GetObject();

            var job = Activator.Create(cls);
            job.Interval = GetSpan(interval);
            job.Name = name;
            job.Options = new Options(options);
            job.Init();

            Pdk.Log(LogLevel.Debug, $"Scheduling task '{name}' every {interval} using class {cls}");
            ScheduleTask(new ScheduledTask(job.Name, job, DateTime.UtcNow, job.Interval, options));
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
            var task = Jobs.FirstOrDefault(t => t.Name == taskInfo.Name);

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
            "daily" => TimeSpan.FromDays(1),
            "hourly" => TimeSpan.FromHours(1),
            "minutely" => TimeSpan.FromMinutes(1),
            "secondly" => TimeSpan.FromSeconds(1),
            "weekly" => TimeSpan.FromDays(7),
            "monthly" => TimeSpan.FromDays(30),
            "yearly" => TimeSpan.FromDays(365),
            _ => throw new ArgumentOutOfRangeException(nameof(span), span, null)
        };
    }
}