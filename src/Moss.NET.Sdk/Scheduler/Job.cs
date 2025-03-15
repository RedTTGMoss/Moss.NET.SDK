using System;

namespace Moss.NET.Sdk.Scheduler;

public abstract class Job
{
    public abstract TimeSpan Interval { get; }
    public virtual string Name { get; }

    public abstract void Run(object data);

    public static void Schedule(Job job)
    {
        TaskScheduler.ScheduleTask(new ScheduledTask(job.Name, job.Run, DateTime.Now, job.Interval));
    }
}