using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;

namespace Totletheyn.Jobs;

public class WikiJob : Job
{
    private readonly LoggerInstance _logger = Log.GetLogger<WikiJob>();
    public override void Run(ref object data)
    {
        data = 22;
        _logger.Info($"Running WikiJob with language '{Options.language}'");
    }
}