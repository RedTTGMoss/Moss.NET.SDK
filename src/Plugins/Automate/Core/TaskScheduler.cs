namespace Automate.Core;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

public class TaskScheduler
{
    private readonly List<ScheduledTask> _tasks = new();
    private readonly Timer _timer;
    private const string FilePath = "tasks.json";

    public TaskScheduler()
    {
        _tasks = LoadTasks();
        _timer = new Timer(CheckTasks, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    public void ScheduleTask(ScheduledTask task)
    {
        _tasks.Add(task);
        SaveTasks();
    }

    private void CheckTasks(object state)
    {
        var now = DateTime.Now;
        foreach (var task in _tasks.Where(t => t.NextRunTime <= now).ToList())
        {
            task.Task();
            task.UpdateNextRunTime();
            SaveTasks();
        }
    }

    private void SaveTasks()
    {
        var json = ScheduledTask.Serialize(_tasks);
        File.WriteAllText(FilePath, json);
    }

    private List<ScheduledTask> LoadTasks()
    {
        if (!File.Exists(FilePath))
        {
            return new List<ScheduledTask>();
        }

        var json = File.ReadAllText(FilePath);
        var tasks = ScheduledTask.Deserialize(json);

        // Reassign the Task actions after deserialization
        foreach (var task in tasks)
        {
            task.Task = GetTaskByName(task.TaskName);
        }

        return tasks;
    }

    private Action GetTaskByName(string taskName)
    {
        return taskName switch
        {
            "Task1" => () => Console.WriteLine("Task 1 executed at " + DateTime.Now),
            "Task2" => () => Console.WriteLine("Task 2 executed at " + DateTime.Now),
            _ => throw new InvalidOperationException($"Unknown task name: {taskName}")
        };
    }
}