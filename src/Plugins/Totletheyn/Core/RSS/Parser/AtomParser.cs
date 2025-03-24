using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds.Atom;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Parser
{
    internal class AtomParser : AbstractXmlFeedParser
    {
        public override BaseFeed Parse(string feedXml, XDocument feedDoc)
        {
            AtomFeed feed = new AtomFeed(feedXml, feedDoc.Root);
            return feed;
        }
    }
}
