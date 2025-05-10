using System;
using System.Collections.Generic;
using System.Linq;
using Totletheyn.Core;
using Totletheyn.Core.RSS;

namespace Totletheyn.Crawlers;

public class PagedOutCrawler : ICrawler
{
    public const string Name = "pagedout";
    private readonly Feed _feed = FeedReader.Read("https://pagedout.institute/rss.xml");

    public bool IsNewIssueAvailable(Issue lastIssue)
    {
        if (lastIssue == null)
        {
            return true;
        }

        var item = _feed.Items.LastOrDefault(x => x.Title == lastIssue.Title);
        if (item == null)
        {
            return true;
        }

        return item.PublishingDate > lastIssue.PublishingDate;
    }

    public IEnumerable<Issue> GetNewIssues(Issue? lastIssue)
    {
        if (lastIssue is null)
        {
            return _feed.Items
                .Select(_ => new Issue(_.Title, _.Link, _.PublishingDate ?? DateTime.Now))
                .ToList();
        }

        var lastItem = _feed.Items.FirstOrDefault(x => x.Title == lastIssue.Title);
        return _feed.Items
            .Where(x => x.PublishingDate > lastItem?.PublishingDate)
            .Select(_ => new Issue(_.Title, _.Link, _.PublishingDate ?? DateTime.Now))
            .ToList();
    }
}