using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds._1._0;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Parser;

internal class Rss10Parser : AbstractXmlFeedParser
{
    public override BaseFeed Parse(string feedXml, XDocument feedDoc)
    {
        var rdf = feedDoc.Root;
        var channel = rdf.GetElement("channel");
        var feed = new Rss10Feed(feedXml, channel);
        return feed;
    }
}