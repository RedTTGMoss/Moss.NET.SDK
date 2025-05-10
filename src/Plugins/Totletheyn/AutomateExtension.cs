using System.Runtime.InteropServices;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;
using Totletheyn.Core.Eventing;
using Totletheyn.Jobs;

namespace Totletheyn;

public class AutomateExtension : MossExtension
{
    private static readonly LoggerInstance Logger = Log.GetLogger<AutomateExtension>();

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_register")]
    public static ulong Register()
    {
        TaskScheduler.Activator.Register<RssJob>("rss");
        TaskScheduler.Activator.Register<CrawlerJob>("crawler");

        Init<AutomateExtension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override void Register(MossState state)
    {
        EventActions.Init(Config);
    }
}