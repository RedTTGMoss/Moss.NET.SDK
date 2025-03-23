using System;
using System.Runtime.CompilerServices;
using Hocon;

namespace Moss.NET.Sdk.Scheduler;

public abstract class Job
{
    public TimeSpan Interval { get; internal set; }
    public string Name { get; internal set; }
    public object Data { get; set; }

    protected internal dynamic Options;

    public virtual void OnInit() {}
    public abstract void Run();
}