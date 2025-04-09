using System.Collections.Generic;
using System.Linq;
using Totletheyn.Core;
using Totletheyn.Core.RSS;

namespace Totletheyn.Crawlers;

public class PagedOutCrawler : ICrawler
{
    public const string Name = "pagedout";
    private Feed feed;

    public PagedOutCrawler()
    {
        feed = FeedReader.Read("https://pagedout.institute/rss.xml");
    }

    public bool IsNewIssueAvailable()
    {
        return true;
    }

    public IEnumerable<Issue> GetNewIssues()
    {
        return feed.Items
            .Select(_ => new Issue(_.Title, _.Link))
            .ToList();
    }
}