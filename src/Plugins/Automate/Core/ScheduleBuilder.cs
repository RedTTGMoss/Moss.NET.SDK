using System;
using Moss.NET.Sdk;
using NiL.JS.Core;

namespace Automate.Core;

class ScheduleBuilder
{
    private static readonly LoggerInstance Logger = Log.GetLogger<ScheduleBuilder>();

    private ScheduledTask _task;

    public static ScheduleBuilder on(string ev)
    {
        Logger.Info("on ");

        var builder = new ScheduleBuilder
        {
            _task = new("s",null, DateTime.Now, GetSpan("day"))
        };
        TaskScheduler.ScheduleTask(builder._task);

        return builder;
    }

    public static ScheduleBuilder every(string span)
    {
        Logger.Info("every ");

        var builder = new ScheduleBuilder
        {
            _task = new("s",null, DateTime.Now, GetSpan(span))
        };

        TaskScheduler.ScheduleTask(builder._task);

        return builder;
    }

    private static TimeSpan GetSpan(string span)
    {
        return span switch
        {
            "day" => TimeSpan.FromDays(1),
            "second" => TimeSpan.FromSeconds(1),
            _ => throw new ArgumentOutOfRangeException(nameof(span), span, null)
        };
    }

    public ScheduleBuilder @do(ICallable callable)
    {
        Logger.Info("do "+ callable);
        _task.Task = callable;

        return this;
    }

    public ScheduleBuilder where(ICallable predicate)
    {
        Logger.Info("where " + predicate);
        _task.Predicate = o => (bool)predicate.Call(JSValue.Undefined, new Arguments()).Value;

        return this;
    }

    public ScheduleBuilder name(string name)
    {
        _task.Name = name;

        return this;
    }
}