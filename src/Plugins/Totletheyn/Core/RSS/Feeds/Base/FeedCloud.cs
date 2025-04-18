﻿using System.Xml.Linq;

namespace Totletheyn.Core.RSS.Feeds.Base;

/// <summary>
///     Cloud object according to Rss 2.0 specification:
///     https://validator.w3.org/feed/docs/rss2.html#ltcloudgtSubelementOfLtchannelgt
/// </summary>
public class FeedCloud
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FeedCloud" /> class.
    ///     default constructor (for serialization)
    /// </summary>
    public FeedCloud()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FeedCloud" /> class.
    ///     Reads a rss feed cloud element based on the xml given in element
    /// </summary>
    /// <param name="element">cloud element as xml</param>
    public FeedCloud(XElement element)
    {
        Domain = element.GetAttributeValue("domain");
        Port = element.GetAttributeValue("port");
        Path = element.GetAttributeValue("path");
        RegisterProcedure = element.GetAttributeValue("registerProcedure");
        Protocol = element.GetAttributeValue("protocol");
    }

    /// <summary>
    ///     The "domain" element
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    ///     The "port" element
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    ///     The "path" element
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    ///     The "registerProcedure" element
    /// </summary>
    public string RegisterProcedure { get; set; }

    /// <summary>
    ///     The "protocol" element
    /// </summary>
    public string Protocol { get; set; }
}