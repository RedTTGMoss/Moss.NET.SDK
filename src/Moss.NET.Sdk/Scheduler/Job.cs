namespace Moss.NET.Sdk.Scheduler;

public abstract class Job
{
    protected internal dynamic Options = null!;
    public TimeSpan Interval { get; internal set; }
    public string? Name { get; internal set; }
    public object? Data { get; set; }

    public virtual void Init()
    {
    }

    public virtual void Shutdown()
    {
    }

    public abstract void Run();
}