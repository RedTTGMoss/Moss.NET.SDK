using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Extism;
using Moss.NET.Sdk.FFI;
using File = System.IO.File;

namespace Moss.NET.Sdk.Scheduler;

public static class TaskScheduler
{
    private static readonly List<ScheduledTask> Tasks = [];

    public static void ScheduleTask(ScheduledTask task)
    {
        if (task.Name is null) task.Name = Tasks.Count.ToString();

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

            task.Task!(task.Data);

            task.UpdateNextRunTime();
        }
    }

    public static void SaveTasks()
    {
        var json = ScheduledTask.Serialize(Tasks);
        File.WriteAllText("extension/.tasks", json);
    }

    public static void LoadTaskInformation()
    {
        string json = "[]";
        try
        {
            json = File.ReadAllText("extension/.tasks");
        }
        catch (Exception)
        {

        }

        var taskInfos = JsonSerializer.Deserialize(json, JsonContext.Default.ListScheduledTask);

        if (taskInfos == null) return;

        // Load task info to appropriate task
        foreach (var taskInfo in taskInfos)
        {
            var task = Tasks.FirstOrDefault(t => t.Name == taskInfo.Name);

            if (task is null)
            {
                Pdk.Log(LogLevel.Error, "task not found: " + taskInfo.Name);
                continue;
            }

            task.NextRunTime = taskInfo.NextRunTime ?? DateTimeOffset.UtcNow;

            task.Interval = taskInfo.Interval;
            task.Name = taskInfo.Name;
            task.Data = taskInfo.Data;
        }
    }
}