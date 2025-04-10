using System.Collections.Generic;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Scheduler;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core;
using Totletheyn.Crawlers;

namespace Totletheyn.Jobs;

public class CrawlerJob : Job
{
    private RestTemplate template = new();
    private Activator<ICrawler> _activator = new();
    private List<ICrawler> _crawlers = [];
    private static readonly LoggerInstance Logger = Log.GetLogger<CrawlerJob>();
    private string inboxId;

    public override void Init()
    {
        Logger.Info("Initializing Crawler");

        _activator.Register<PagedOutCrawler>(PagedOutCrawler.Name);
        _activator.Register<FrauenhoferCrawler>(FrauenhoferCrawler.Name);

        inboxId = MossConfig.Get<string>("inbox");

        foreach (var crawler in (string[])Options.providers)
        {
            _crawlers.Add(_activator.Create(crawler));
        }

        Logger.Info("Crawlers initialized: ");
    }

    public override void Run()
    {
        Logger.Info("Running Crawler");

        List<Issue> issues = [];
        foreach (var crawler in _crawlers)
        {
            if (crawler.IsNewIssueAvailable())
            {
                issues.AddRange(crawler.GetNewIssues());
            }
        }

        Logger.Info($"CrawlerJob has {issues.Count} new issues");
        foreach (var issue in issues)
        {
            Logger.Info($"downloading {issue.Title}");
            Base64 content = template.Exchange(issue.PdfUrl).Body;
            Logger.Info($"uploading {issue.Title}");

            var pdf = new PdfNotebook(issue.Title, content, inboxId);
            //pdf.Upload();
        }
    }
}
