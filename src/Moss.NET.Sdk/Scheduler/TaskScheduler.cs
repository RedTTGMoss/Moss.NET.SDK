using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.Scheduler;

public static class TaskScheduler
{
    private static readonly List<ScheduledTask> Tasks = [];

    public static void ScheduleTask(ScheduledTask task)
    {
        if (task.Name is null) task.Name = Tasks.Count.ToString();

        Tasks.Add(task);
        SaveTasks();
    }

    internal static void CheckTasks()
    {
        var now = DateTime.Now;
        foreach (var task in Tasks
                     .Where(t => t.NextRunTime <= now)
                     .Where(task => task.Predicate is null || task.Predicate(task))
                     .ToArray())
        {
            task.Task(task.Data);

            task.UpdateNextRunTime();
        }

        SaveTasks();
    }

    private static void SaveTasks()
    {
        var json = ScheduledTask.Serialize(Tasks);
        Config.Set("tasks", json);
    }

    public static void LoadTaskInformation()
    {
        var taskInfos = JsonSerializer.Deserialize(Config.Get<string>("tasks"), JsonContext.Default.ListScheduledTask);

        if (taskInfos == null) return;

        // Load task info to appropriate task
        foreach (var taskInfo in taskInfos)
        {
            var task = Tasks.First(t => t.Name == taskInfo.Name);

            task.NextRunTime = taskInfo.NextRunTime;
            task.Interval = taskInfo.Interval;
            task.Name = taskInfo.Name;
        }
    }
}