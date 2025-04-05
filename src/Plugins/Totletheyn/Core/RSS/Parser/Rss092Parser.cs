using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds._0._92;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Parser;

internal class Rss092Parser : AbstractXmlFeedParser
{
    public override BaseFeed Parse(string feedXml, XDocument feedDoc)
    {
        var rss = feedDoc.Root;
        var channel = rss.GetElement("channel");
        var feed = new Rss092Feed(feedXml, channel);
        return feed;
    }
}