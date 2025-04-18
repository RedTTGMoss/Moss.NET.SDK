using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Feeds.Atom;

/// <summary>
///     Atom 1.0 feed item object according to specification: https://validator.w3.org/feed/docs/atom.html
/// </summary>
public class AtomFeedItem : BaseFeedItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AtomFeedItem" /> class.
    ///     default constructor (for serialization)
    /// </summary>
    public AtomFeedItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AtomFeedItem" /> class.
    ///     Reads an atom feed based on the xml given in item
    /// </summary>
    /// <param name="item">feed item as xml</param>
    public AtomFeedItem(XElement item)
        : base(item)
    {
        Link = item.GetElement("link")?.Attribute("href")?.Value;

        Author = new AtomPerson(item.GetElement("author"));

        var categories = item.GetElements("category");
        Categories = categories.Select(x => (string)x.Attribute("term")).ToList();

        Content = item.GetValue("content").HtmlDecode();
        Contributor = new AtomPerson(item.GetElement("contributor"));
        Id = item.GetValue("id");

        PublishedDateString = item.GetValue("published");
        PublishedDate = Helpers.TryParseDateTime(PublishedDateString);
        Links = item.GetElements("link").Select(x => new AtomLink(x)).ToList();

        Rights = item.GetValue("rights");
        Source = item.GetValue("source");
        Summary = item.GetValue("summary");

        UpdatedDateString = item.GetValue("updated");
        UpdatedDate = Helpers.TryParseDateTime(UpdatedDateString);
    }

    /// <summary>
    ///     The "author" element
    /// </summary>
    public AtomPerson Author { get; set; }

    /// <summary>
    ///     All "category" elements
    /// </summary>
    public ICollection<string> Categories { get; set; }

    /// <summary>
    ///     The "content" element
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    ///     The "contributor" element
    /// </summary>
    public AtomPerson Contributor { get; set; }

    /// <summary>
    ///     The "id" element
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     The "published" date as string
    /// </summary>
    public string PublishedDateString { get; set; }

    /// <summary>
    ///     The "published" element as DateTime. Null if parsing failed or published is empty.
    /// </summary>
    public DateTime? PublishedDate { get; set; }

    /// <summary>
    ///     The "rights" element
    /// </summary>
    public string Rights { get; set; }

    /// <summary>
    ///     The "source" element
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    ///     The "summary" element
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    ///     The "updated" element
    /// </summary>
    public string UpdatedDateString { get; set; }

    /// <summary>
    ///     The "updated" element as DateTime. Null if parsing failed or updated is empty
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    ///     All "link" elements
    /// </summary>
    public ICollection<AtomLink> Links { get; set; }

    /// <inheritdoc />
    internal override FeedItem ToFeedItem()
    {
        var fi = new FeedItem(this)
        {
            Author = Author?.ToString(),
            Categories = Categories,
            Content = Content,
            Description = Summary,
            Id = Id,
            PublishingDate = PublishedDate,
            PublishingDateString = PublishedDateString
        };
        return fi;
    }
}