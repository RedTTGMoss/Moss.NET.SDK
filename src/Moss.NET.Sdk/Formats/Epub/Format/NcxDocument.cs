﻿using System.Xml.Linq;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.Formats.Epub.Format;

internal static class NcxElements
{
    public static readonly XName Ncx = Constants.NcxNamespace + "ncx";
    public static readonly XName Head = Constants.NcxNamespace + "head";
    public static readonly XName Meta = Constants.NcxNamespace + "meta";
    public static readonly XName DocTitle = Constants.NcxNamespace + "docTitle";
    public static readonly XName DocAuthor = Constants.NcxNamespace + "docAuthor";
    public static readonly XName Text = Constants.NcxNamespace + "text";
    public static readonly XName NavMap = Constants.NcxNamespace + "navMap";
    public static readonly XName NavPoint = Constants.NcxNamespace + "navPoint";
    public static readonly XName NavList = Constants.NcxNamespace + "navList";
    public static readonly XName PageList = Constants.NcxNamespace + "pageList";
    public static readonly XName NavInfo = Constants.NcxNamespace + "navInfo";
    public static readonly XName PageTarget = Constants.NcxNamespace + "pageTarget";
    public static readonly XName NavLabel = Constants.NcxNamespace + "navLabel";
    public static readonly XName NavTarget = Constants.NcxNamespace + "navTarget";
    public static readonly XName Content = Constants.NcxNamespace + "content";
}

/// <summary>
///     DAISY’s Navigation Center eXtended (NCX)
/// </summary>
public class NcxDocument
{
    public IList<NcxMeta> Meta { get; internal set; } = new List<NcxMeta>();
    public string DocTitle { get; internal set; }
    public string DocAuthor { get; internal set; }
    public NcxNapMap NavMap { get; internal set; } = new(); // <navMap> is a required element in NCX.
    public NcxPageList PageList { get; internal set; }
    public NcxNavList NavList { get; internal set; }
}

public class NcxMeta
{
    public string Name { get; internal set; }
    public string Content { get; internal set; }
    public string Scheme { get; internal set; }

    internal static class Attributes
    {
        public static readonly XName Name = "name";
        public static readonly XName Content = "content";
        public static readonly XName Scheme = "scheme";
    }
}

public class NcxNapMap
{
    /// <summary>
    ///     Populated only when an EPUB with NCX is read.
    /// </summary>
    public XElement Dom { get; internal set; }

    public IList<NcxNavPoint> NavPoints { get; internal set; } = new List<NcxNavPoint>();
}

public class NcxNavPoint
{
    public string Id { get; internal init; }
    public string Class { get; internal init; }
    public int? PlayOrder { get; internal init; }

    // NavLabelText and ContentSrc are flattened elements for convenience.
    // In case <navLabel> or <content/> need to carry more data, then they should have a dedicated model created.
    public string NavLabelText { get; internal init; }

    public string? ContentSrc { get; internal init; }
    public IList<NcxNavPoint> NavPoints { get; internal init; } = new List<NcxNavPoint>();

    public override string ToString()
    {
        return $"Id: {Id}, ContentSource: {ContentSrc}";
    }

    internal static class Attributes
    {
        public static readonly XName Id = "id";
        public static readonly XName Class = "class";
        public static readonly XName PlayOrder = "playOrder";
        public static readonly XName ContentSrc = "src";
    }
}

public enum NcxPageTargetType
{
    Front = 1,
    Normal,
    Special,
    Body
}

public class NcxPageList
{
    public NcxNavInfo NavInfo { get; internal init; }

    public IList<NcxPageTarget> PageTargets { get; internal init; } = new List<NcxPageTarget>();
}

public class NcxNavInfo
{
    public string Text { get; internal set; }
}

public class NcxPageTarget
{
    public string Id { get; internal init; }
    public int? Value { get; internal init; }
    public string Class { get; internal init; }
    public NcxPageTargetType? Type { get; internal init; }
    public string NavLabelText { get; internal init; }
    public string ContentSrc { get; internal init; }

    internal static class Attributes
    {
        public static readonly XName Id = "id";
        public static readonly XName Class = "class";
        public static readonly XName Type = "type";
        public static readonly XName Value = "value";
        public static readonly XName ContentSrc = "src";
    }
}

public class NcxNavList
{
    public string Id { get; internal set; }
    public string Class { get; internal set; }
    public string Label { get; internal set; }
    public IList<NcxNavTarget> NavTargets { get; internal set; } = new List<NcxNavTarget>();
}

public class NcxNavTarget
{
    public string Id { get; internal set; }
    public string Class { get; internal set; }
    public int? PlayOrder { get; internal set; }
    public string Label { get; internal set; }
    public string ContentSource { get; internal set; }
}