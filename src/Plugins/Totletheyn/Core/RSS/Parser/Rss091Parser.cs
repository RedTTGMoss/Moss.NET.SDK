using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds._0._91;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Parser
{
    internal class Rss091Parser : AbstractXmlFeedParser
    {
        public override BaseFeed Parse(string feedXml, XDocument feedDoc)
        {
            var rss = feedDoc.Root;
            var channel = rss.GetElement("channel");
            Rss091Feed feed = new Rss091Feed(feedXml, channel);
            return feed;
        }
    }
}
