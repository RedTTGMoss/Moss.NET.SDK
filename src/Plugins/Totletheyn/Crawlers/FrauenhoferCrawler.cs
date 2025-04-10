using Totletheyn.Core;
using Moss.NET.Sdk.Core;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Linq;
using System.Collections.Generic;

namespace Totletheyn.Crawlers;

public class FrauenhoferCrawler : ICrawler
{
    public const string Name = "frauenhofer";

    private readonly RestTemplate template = new();
    private readonly HtmlDocument document = new();


    public FrauenhoferCrawler()
    {
        var content = template
            .Exchange("https://www.fraunhofer.de/de/mediathek/publikationen/fraunhofer-magazin.html")
            .Body
            .ReadString();

        document.LoadHtml(content);
    }

    public bool IsNewIssueAvailable()
    {
        return true;
    }

    public IEnumerable<Issue> GetNewIssues()
    {
        var titles = document.QuerySelectorAll("h3.teaser-default__text-headline").Select(_ => _.InnerText).ToList();
        var links = document.QuerySelectorAll(".file-pdf > a").Select(_ => _.Attributes["href"].Value).ToList();

        var issues = new List<Issue>();
        for (int i = 0; i < titles.Count; i++)
        {
            issues.Add(new Issue(titles[i], links[i]));
        }

        return issues;
    }
}