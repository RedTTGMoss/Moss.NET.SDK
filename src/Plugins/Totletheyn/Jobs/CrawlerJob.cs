using System.Collections.Generic;
using System.Linq;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Scheduler;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core;
using Totletheyn.Crawlers;

namespace Totletheyn.Jobs;

public class CrawlerJob : Job
{
    private readonly RestTemplate _template = new();
    private readonly Activator<ICrawler> _activator = new();
    private readonly List<ICrawler> _crawlers = [];
    private static readonly LoggerInstance Logger = Log.GetLogger<CrawlerJob>();
    private string _inboxId;

    public override void Init()
    {
        Logger.Info("Initializing Crawler");

        _activator.Register<PagedOutCrawler>(PagedOutCrawler.Name);
        _activator.Register<FrauenhoferCrawler>(FrauenhoferCrawler.Name);

        _inboxId = MossConfig.Get<string>("inbox");

        if (Options.providers is null){
            Logger.Error("No providers specified");
            return;
        }

        foreach (var crawler in Options.providers)
        {
            _crawlers.Add(_activator.Create(crawler));
        }

        Logger.Info("Crawlers initialized: ");
    }

    public override void Run()
    {
        Logger.Info("Running Crawlers");

        List<Issue> issues = [];
        foreach (var crawler in _crawlers)
        {
            Logger.Info("Running Crawler: " + _activator.GetName(crawler));
            var lastIssue = MossExtension.Instance!.Cache.Get<Issue>($"crawler.{_activator.GetName(crawler)}");
            Logger.Info("Last issue: " + lastIssue?.Title);

            if (crawler.IsNewIssueAvailable(lastIssue))
            {
                Logger.Info("New issues available");
                issues.AddRange(crawler.GetNewIssues(lastIssue));

                MossExtension.Instance!.Cache.Set($"crawler.{_activator.GetName(crawler)}", issues.Last());
            }
        }

        Logger.Info($"CrawlerJob has {issues.Count} new issues");
        foreach (var issue in issues)
        {
            Logger.Info($"downloading {issue.Title}");
            Base64 content = _template.Exchange(issue.PdfUrl).Body;
            Logger.Info($"uploading {issue.Title}");

            var pdf = new PdfNotebook(issue.Title, content);
            pdf.MoveTo(_inboxId);
            //pdf.Upload();


        }
    }
}
