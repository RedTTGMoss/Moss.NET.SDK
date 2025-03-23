using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;

namespace SdkTesterPlugin;

public class TestJob : Job
{
    private static readonly LoggerInstance Logger = Log.GetLogger<TestJob>();

    public override void Run()
    {
        Logger.Info("TestJob running");
    }
}