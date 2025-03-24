using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds.Base;
using Totletheyn.Core.RSS.Feeds.MediaRSS;

namespace Totletheyn.Core.RSS.Parser
{
    internal class MediaRssParser : AbstractXmlFeedParser
    {
        public override BaseFeed Parse(string feedXml, XDocument feedDoc)
        {
            var rss = feedDoc.Root;
            var channel = rss.GetElement("channel");
            MediaRssFeed feed = new MediaRssFeed(feedXml, channel);
            return feed;
        }
    }
}