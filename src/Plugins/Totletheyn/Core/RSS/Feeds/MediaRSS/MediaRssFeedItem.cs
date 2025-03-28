﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Totletheyn.Core.RSS.Feeds.Base;

namespace Totletheyn.Core.RSS.Feeds.MediaRSS
{
    /// <summary>
    /// RSS 2.0 feed item accoring to specification: https://validator.w3.org/feed/docs/rss2.html
    /// </summary>
    public class MediaRssFeedItem : BaseFeedItem
    {
        /// <summary>
        /// The "description" field of the feed item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The "author" field of the feed item
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The "comments" field of the feed item
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// The "enclosure" field
        /// </summary>
        public FeedItemEnclosure Enclosure { get; set; }

        /// <summary>
        /// The "guid" field
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// The "pubDate" field
        /// </summary>
        public string PublishingDateString { get; set; }

        /// <summary>
        /// The "pubDate" field as DateTime. Null if parsing failed or pubDate is empty.
        /// </summary>
        public DateTime? PublishingDate { get; set; }

        /// <summary>
        /// The "source" field
        /// </summary>
        public FeedItemSource Source { get; set; }

        /// <summary>
        /// All entries "category" entries
        /// </summary>
        public ICollection<string> Categories { get; set; }

        /// <summary>
        /// All entries from the "media:content" elements.
        /// </summary>
        public ICollection<Media> Media { get; set; }

        /// <summary>
        /// All entries from the "media:group" elements. 
        /// </summary>
        public ICollection<MediaGroup> MediaGroups { get; set; }

        /// <summary>
        /// The "content:encoded" field
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// All elements starting with "dc:"
        /// </summary>
        public DublinCore DC { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRssFeedItem"/> class.
        /// default constructor (for serialization)
        /// </summary>
        public MediaRssFeedItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRssFeedItem"/> class.
        /// Reads a new feed item element based on the given xml item
        /// </summary>
        /// <param name="item">the xml containing the feed item</param>
        public MediaRssFeedItem(XElement item)
            : base(item)
        {
             
            this.Comments = item.GetValue("comments");
            this.Author = item.GetValue("author");
            this.Enclosure = new FeedItemEnclosure(item.GetElement("enclosure"));
            this.PublishingDateString = item.GetValue("pubDate");
            this.PublishingDate = Helpers.TryParseDateTime(this.PublishingDateString);
            this.DC = new DublinCore(item);
            this.Source = new FeedItemSource(item.GetElement("source"));

            var media = item.GetElements("media", "content");
            this.Media = media.Select(x => new Media(x)).ToList();

            var mediaGroups = item.GetElements("media", "group");
            this.MediaGroups = mediaGroups.Select(x => new MediaGroup(x)).ToList();

            var categories = item.GetElements("category");
            this.Categories = categories.Select(x => x.GetValue()).ToList();

            this.Guid = item.GetValue("guid");
            this.Description = item.GetValue("description");
            this.Content = item.GetValue("content:encoded")?.HtmlDecode();
        }

        /// <inheritdoc/>
        internal override FeedItem ToFeedItem()
        {
            FeedItem fi = new FeedItem(this)
            {
                Author = this.Author,
                Categories = this.Categories,
                Content = this.Content,
                Description = this.Description,
                Id = this.Guid,
                PublishingDate = this.PublishingDate,
                PublishingDateString = this.PublishingDateString
            };
            return fi;
        }
    }
}
