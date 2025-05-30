﻿using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Feeds._0._91;

/// <summary>
///     Rss 0.91 Feed Image according to specification: http://www.rssboard.org/rss-0-9-1-netscape#image
/// </summary>
public class Rss091FeedImage : FeedImage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Rss091FeedImage" /> class.
    ///     default constructor (for serialization)
    /// </summary>
    public Rss091FeedImage()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rss091FeedImage" /> class.
    ///     Creates this object based on the xml in the XElement parameter.
    /// </summary>
    /// <param name="element">feed image as xml</param>
    public Rss091FeedImage(XElement element)
        : base(element)
    {
        Description = element.GetValue("description");
        Width = Helpers.TryParseInt(element.GetValue("width"));
        Height = Helpers.TryParseInt(element.GetValue("height"));
    }

    /// <summary>
    ///     The "description" element
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     The "width" element
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    ///     The "height" element
    /// </summary>
    public int? Height { get; set; }
}