using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Parser
{
    internal interface IFeedParser
    {
        BaseFeed Parse(string feedXml);
    }
}
