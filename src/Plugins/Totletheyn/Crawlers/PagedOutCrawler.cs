using System.Collections.Generic;
using System.Linq;
using Totletheyn.Core;
using Totletheyn.Core.RSS;

namespace Totletheyn.Crawlers;

public class PagedOutCrawler : ICrawler
{
    private Feed feed;

    public PagedOutCrawler()
    {
        feed = FeedReader.Read("https://pagedout.institute/rss.xml");
    }

    public bool IsNewIssueAvailable()
    {

        return true;
    }

    public IEnumerable<Issue> GetNewIssues(List<string> lastIssueTitles)
    {
        foreach (var item in feed.Items)
        {
            //yield return new(item.Title, item.Link);
        }

        return feed.Items.Select(_ => new Issue(_.Title, _.Link))
            .Where(issue => !lastIssueTitles.Contains(issue.Title))
            .ToList();
    }
}