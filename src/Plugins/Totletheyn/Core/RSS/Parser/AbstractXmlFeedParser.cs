using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Parser
{
    internal abstract class AbstractXmlFeedParser : IFeedParser
    {
        public BaseFeed Parse(string feedXml)
        {
            XDocument feedDoc = XDocument.Parse(feedXml);

            return this.Parse(feedXml, feedDoc);
        }

        public abstract BaseFeed Parse(string feedXml, XDocument feedDoc);
    }
}
