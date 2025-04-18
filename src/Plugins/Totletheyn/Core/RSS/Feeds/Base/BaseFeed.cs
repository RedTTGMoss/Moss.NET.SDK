﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace Totletheyn.Core.RSS.Feeds.Base;

/// <summary>
///     BaseFeed object which contains the basic properties that each feed has.
/// </summary>
public abstract class BaseFeed
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseFeed" /> class.
    ///     default constructor (for serialization)
    /// </summary>
    protected BaseFeed()
    {
        Items = new List<BaseFeedItem>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseFeed" /> class.
    ///     Reads a base feed based on the xml given in element
    /// </summary>
    /// <param name="feedXml">the entire feed xml as string</param>
    /// <param name="channel">the "channel" element in the xml as XElement</param>
    protected BaseFeed(string feedXml, XElement channel)
        : this()
    {
        OriginalDocument = feedXml;

        Title = channel.GetValue("title");
        Link = channel.GetValue("link");
        Element = channel;
    }

    /// <summary>
    ///     The title of the feed
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///     The link (url) to the feed
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    ///     The items that are in the feed
    /// </summary>
    public ICollection<BaseFeedItem> Items { get; set; }

    /// <summary>
    ///     Gets the whole, original feed as string
    /// </summary>
    public string OriginalDocument { get; private set; }

    /// <summary>
    ///     Gets the underlying XElement in order to allow reading properties that are not available in the class itself
    /// </summary>
    public XElement Element { get; }

    /// <summary>
    ///     creates the generic <see cref="Feed" /> object.
    /// </summary>
    /// <returns>Feed</returns>
    public abstract Feed ToFeed();
}