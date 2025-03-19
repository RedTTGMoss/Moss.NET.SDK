using System;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;
using NiL.JS.Core;

namespace Totletheyn.Core;

internal class ScheduleBuilder
{
    private static readonly LoggerInstance Logger = Log.GetLogger<ScheduleBuilder>();

    private ScheduledTask _task;

    public static ScheduleBuilder on(string ev)
    {
        Logger.Info("on ");

        var builder = new ScheduleBuilder
        {
            _task = new ScheduledTask("s", null, DateTimeOffset.UtcNow, GetSpan("day"))
        };
        TaskScheduler.ScheduleTask(builder._task);

        return builder;
    }

    public static ScheduleBuilder every(string span)
    {
        var builder = new ScheduleBuilder
        {
            _task = new ScheduledTask(null, null, DateTimeOffset.UtcNow, GetSpan(span))
        };

        TaskScheduler.ScheduleTask(builder._task);

        return builder;
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

    public ScheduleBuilder @do(ICallable callable)
    {
        _task.Task = _ => callable.Call(JSValue.Undefined, new Arguments { _ });

        return this;
    }

    public ScheduleBuilder where(ICallable predicate)
    {
        _task.Predicate = o => (bool)predicate.Call(JSValue.Undefined, new Arguments()).Value;

        return this;
    }

    public ScheduleBuilder name(string name)
    {
        _task.Name = name;

        return this;
    }
}