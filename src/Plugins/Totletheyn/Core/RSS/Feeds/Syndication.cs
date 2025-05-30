﻿using System.Xml.Linq;

namespace Totletheyn.Core.RSS.Feeds;

/// <summary>
///     The parsed syndication elements. Those are all elements that start with "sy:"
/// </summary>
public class Syndication
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Syndication" /> class.
    ///     default constructor (for serialization)
    /// </summary>
    public Syndication()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Syndication" /> class.
    ///     Creates the object based on the xml in the given XElement
    /// </summary>
    /// <param name="xelement">syndication element as xml</param>
    public Syndication(XElement xelement)
    {
        UpdateBase = xelement.GetValue("sy:updateBase");
        UpdateFrequency = xelement.GetValue("sy:updateFrequency");
        UpdatePeriod = xelement.GetValue("sy:updatePeriod");
    }

    /// <summary>
    ///     The "updatePeriod" element
    /// </summary>
    public string UpdatePeriod { get; set; }

    /// <summary>
    ///     The "updateFrequency" element
    /// </summary>
    public string UpdateFrequency { get; set; }

    /// <summary>
    ///     The "updateBase" element
    /// </summary>
    public string UpdateBase { get; set; }
}