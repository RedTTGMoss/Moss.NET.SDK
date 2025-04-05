using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Feeds._1._0;

/// <summary>
///     Rss 1.0 Feed image according to specification: http://web.resource.org/rss/1.0/spec
/// </summary>
public class Rss10FeedImage : FeedImage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Rss10FeedImage" /> class.
    ///     default constructor (for serialization)
    /// </summary>
    public Rss10FeedImage()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rss10FeedImage" /> class.
    ///     Reads a rss 1.0 feed image based on the xml given in element
    /// </summary>
    /// <param name="element">feed image as xml</param>
    public Rss10FeedImage(XElement element)
        : base(element)
    {
        About = element.GetAttribute("rdf:about").GetValue();
    }

    /// <summary>
    ///     The "about" attribute of the element
    /// </summary>
    public string About { get; set; }
}