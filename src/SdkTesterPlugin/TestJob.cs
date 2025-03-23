using Hocon;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;

namespace SdkTesterPlugin;

public class TestJob : Job
{
    private static readonly LoggerInstance Logger = Log.GetLogger<TestJob>();

    public override void Run(object data)
    {
        Logger.Info("TestJob running");
    }
}