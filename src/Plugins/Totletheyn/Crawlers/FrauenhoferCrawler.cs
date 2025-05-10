using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Totletheyn.Core;

namespace Totletheyn.Crawlers;

public class FrauenhoferCrawler : ICrawler
{
    public const string Name = "frauenhofer";
    private readonly HtmlDocument _document = new();

    private readonly RestTemplate _template = new();

    public FrauenhoferCrawler()
    {
        var content = _template
            .Exchange("https://www.fraunhofer.de/de/mediathek/publikationen/fraunhofer-magazin.html")
            .Body
            .ReadString();

        _document.LoadHtml(content);
    }

    public bool IsNewIssueAvailable(Issue lastIssue)
    {
        return true;
    }

    public IEnumerable<Issue> GetNewIssues(Issue lastIssue)
    {
        var titles = _document.QuerySelectorAll("h3.teaser-default__text-headline").Select(_ => _.InnerText).ToList();
        var links = _document.QuerySelectorAll(".file-pdf > a").Select(_ => _.Attributes["href"].Value).ToList();

        var issues = new List<Issue>();
        for (var i = 0; i < titles.Count; i++) issues.Add(new Issue(titles[i], links[i], DateTime.Now));

        return issues.TakeLast(1);
    }
}