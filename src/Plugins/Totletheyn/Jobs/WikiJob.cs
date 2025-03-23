using System;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;
using SharpWiki;
using SharpWiki.Models;

namespace Totletheyn.Jobs;

public class WikiJob : Job
{
    private readonly LoggerInstance _logger = Log.GetLogger<WikiJob>();
    //private SharpWikiClient _client;

    public override void OnInit()
    {
        //_client = new SharpWikiClient(GetClientConfig(), HttpClientFactory.Factory);
    }

    private SharpWikiClientOptions GetClientConfig()
    {
        return new SharpWikiClientOptions
        {
            ApiUserAgent = "Totletheyn",
            Language = Enum.Parse<WikiLanguage>(Options.language, true)
        };
    }

    public override void Run()
    {
        Data = 22;
        _logger.Info($"Running WikiJob with language '{Options.language}'");
        //var src = _client.GetPageSourceAsync("reMarkable").GetAwaiter().GetResult();

        //_logger.Info($"Wiki Html: {src.Html}");
    }
}