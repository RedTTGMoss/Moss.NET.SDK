using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds._2._0;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Parser
{
    internal class Rss20Parser : AbstractXmlFeedParser
    {
        public override BaseFeed Parse(string feedXml, XDocument feedDoc)
        {
            var rss = feedDoc.Root;
            var channel = rss.GetElement("channel");
            Rss20Feed feed = new Rss20Feed(feedXml, channel);
            return feed;
        }
    }
}