using System.Text.Json;
using Moss.NET.Sdk;
using NiL.JS.Core;

namespace Automate.Core;

using System;
using System.Collections.Generic;
using System.Linq;

public static class TaskScheduler
{
    private static readonly List<ScheduledTask> Tasks = [];

    public static void ScheduleTask(ScheduledTask task)
    {
        if (task.Name is null)
        {
            task.Name = Tasks.Count.ToString();
        }

        Tasks.Add(task);
        SaveTasks();
    }

    public static void CheckTasks()
    {
        var now = DateTime.Now;
        foreach (var task in Tasks.Where(t => t.NextRunTime <= now).ToList())
        {
            if (task.Predicate is not null && !task.Predicate(task))
            {
                continue;
            }

            task.Task.Call(JSValue.Undefined, new Arguments());

            task.UpdateNextRunTime();
            SaveTasks();
        }
    }

    private static void SaveTasks()
    {
        var json = ScheduledTask.Serialize(Tasks);
        Config.Set("tasks", json);
    }

    public static void LoadTaskInformation()
    {
        var taskInfos = JsonSerializer.Deserialize(Config.Get<string>("tasks"), JsonContext.Default.ListScheduledTask);

        if (taskInfos == null)
        {
            return;
        }

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