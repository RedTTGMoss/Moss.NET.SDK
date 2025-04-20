using System.Xml.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.Formats.Epub.Format;

internal static class NavElements
{
    public const string Html = "html";

    public const string Head = "head";
    public const string Title = "title";
    public const string Link = "link";
    public const string Meta = "meta";

    public const string Body = "body";
    public const string Nav = "nav";
    public const string Ol = "ol";
    public const string Li = "li";
    public const string A = "a";
}

public class NavDocument
{
    public NavHead Head { get; internal init; } = new();
    public NavBody Body { get; internal init; } = new();
}

public class NavHead
{
    /// <summary>
    ///     Instantiated only when the EPUB was read.
    /// </summary>
    internal XElement Dom { get; set; }

    public string Title { get; internal set; }
    public IList<NavHeadLink> Links { get; internal set; } = new List<NavHeadLink>();
    public IList<NavMeta> Metas { get; internal set; } = new List<NavMeta>();
}

public class NavHeadLink
{
    public string Href { get; internal set; }
    public string Rel { get; internal set; }
    public string Type { get; internal set; }
    public string Class { get; internal set; }
    public string Title { get; internal set; }
    public string Media { get; internal set; }

    internal static class Attributes
    {
        public static readonly XName Href = "href";
        public static readonly XName Rel = "rel";
        public static readonly XName Type = "type";
        public static readonly XName Class = "class";
        public static readonly XName Title = "title";
        public static readonly XName Media = "media";
    }
}

public class NavMeta
{
    public string Name { get; internal set; }
    public string Content { get; internal set; }
    public string Charset { get; internal set; }

    internal static class Attributes
    {
        public static readonly XName Name = "name";
        public static readonly XName Content = "content";
        public static readonly XName Charset = "charset";
    }
}

public class NavBody
{
    /// <summary>
    ///     Instantiated only when the EPUB was read.
    /// </summary>
    internal XElement Dom { get; set; }

    public IList<NavNav> Navs { get; internal init; } = new List<NavNav>();
}

public class NavNav
{
    /// <summary>
    ///     Instantiated only when the EPUB was read.
    /// </summary>
    internal XElement Dom { get; init; }

    public string Type { get; internal init; }
    public string Id { get; internal set; }
    public string Class { get; internal set; }
    public string Hidden { get; internal set; }

    internal static class Attributes
    {
        public static readonly XName Id = "id";
        public static readonly XName Class = "class";
        public static readonly XName Type = Constants.OpsNamespace + "type";
        public static readonly XName Hidden = Constants.OpsNamespace + "hidden";

        internal static class TypeValues
        {
            public const string Toc = "toc";
            public const string Landmarks = "landmarks";
            public const string PageList = "page-list";
        }
    }
}