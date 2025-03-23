using System;
using Hocon;

namespace Moss.NET.Sdk.Scheduler;

public abstract class Job
{
    public TimeSpan Interval { get; internal set; }
    public string Name { get; internal set; }

    protected internal dynamic Options;

    public abstract void Run(object data);
}