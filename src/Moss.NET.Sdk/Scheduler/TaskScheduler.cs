using Extism;
using Hocon;
using Moss.NET.Sdk.Core;
using PolyType;

namespace Moss.NET.Sdk.Scheduler;

[GenerateShape]
partial class JobsCollection : List<ScheduledTask>;

public static class TaskScheduler
{
    private static HoconObject _config = null!;
    private static JobsCollection Jobs = [];
    public static readonly Activator<Job> Activator = new();
    public static bool IsEnabled => MossExtension.Config!.GetBoolean("scheduler.enabled", true);
    private static readonly LoggerInstance Logger = Log.GetLogger(nameof(TaskScheduler));

    public static void ScheduleTask(ScheduledTask task)
    {
        task.Name ??= Jobs.Count.ToString();

        Jobs.Add(task);
    }

    internal static void RunJobs()
    {
        if (!IsEnabled) return;

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
        if (!IsEnabled) return;

        foreach (var job in Jobs)
        {
            job.Data = job.Job.Data;
            job.Job.Shutdown();
        }

        MossExtension.Instance!.Cache.Set("scheduler", Jobs);
    }

    public static void Init()
    {
        if (!IsEnabled) return;

        ReadJobConfig();
    }

    private static void ReadJobConfig()
    {
        if (MossExtension.Config is null) return;

        _config = MossExtension.Config.GetObject("scheduler", null);
        if (_config is null) return;

        var jobsDefinition = _config.GetObject("jobs");
        if (jobsDefinition is null) return;

        foreach (var jobInfo in jobsDefinition)
        {
            var obj = jobInfo.Value.GetObject();

            if (!obj["enabled"].GetBoolean())
            {
                Pdk.Log(LogLevel.Debug, $"Task '{jobInfo.Key}' is disabled");
                continue;
            }

            if (obj["class"] == null)
            {
                Pdk.Log(LogLevel.Error, $"Task '{jobInfo.Key}' has no class");
                continue;
            }

            var cls = obj["class"].GetString();
            var interval = obj["interval"].GetString();
            var name = jobInfo.Key;
            var options = obj["options"].GetObject();

            var job = Activator.Create(cls);
            job.Interval = GetSpan(interval);
            job.Name = name;
            job.Options = new Options(options);
            job.Init();

            Pdk.Log(LogLevel.Debug, $"Scheduling task '{name}' every {interval} using class {cls}");
            ScheduleTask(new ScheduledTask(job.Name, job, DateTime.UtcNow, job.Interval, options));
        }

        AssociateJobInfo();
    }

    private static void AssociateJobInfo()
    {
        if (!Jobs.Any()) return;

        // Load task info to appropriate job
        foreach (var job in MossExtension.Instance!.Cache.Get<JobsCollection>("scheduler")!)
        {
            var task = Jobs.FirstOrDefault(t => t.Name == job.Name);

            if (task is null)
            {
                Pdk.Log(LogLevel.Error, "job not found: " + job.Name);
                continue;
            }

            task.NextRunTime = job.NextRunTime ?? DateTimeOffset.UtcNow;

            task.Interval = job.Interval;
            task.Name = job.Name;
            task.Data = job.Data;
        }
    }

    private static TimeSpan GetSpan(string span)
    {
        if (TimeSpan.TryParse(span, out var result)) return result;

        var unit = span[^1];
        if (int.TryParse(span[..^1], out var value))
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