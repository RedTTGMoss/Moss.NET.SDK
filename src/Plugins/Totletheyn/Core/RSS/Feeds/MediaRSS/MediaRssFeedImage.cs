﻿using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds._0._91;

namespace Totletheyn.Core.RSS.Feeds.MediaRSS;

/// <summary>
///     Rss 2.0 Image according to specification:
///     https://validator.w3.org/feed/docs/rss2.html#ltimagegtSubelementOfLtchannelgt
/// </summary>
public class MediaRssFeedImage : Rss091FeedImage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MediaRssFeedImage" /> class.
    ///     default constructor (for serialization)
    /// </summary>
    public MediaRssFeedImage()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MediaRssFeedImage" /> class.
    ///     Reads a rss 2.0 feed image based on the xml given in element
    /// </summary>
    /// <param name="element">feed image as xml</param>
    public MediaRssFeedImage(XElement element)
        : base(element)
    {
    }
}