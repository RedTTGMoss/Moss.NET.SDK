using System;
using System.Collections.Generic;
using System.Linq;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using Totletheyn.Core.RSS;
using Totletheyn.DataSources;
using Totletheyn.DataSources.Crypto;
using Totletheyn.DataSources.Weather;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.Jobs;

public class Newspaper
{
    private List<Feed> Feeds { get; } = [];

    private readonly PdfDocumentBuilder _builder = new();
    private readonly int Issue;

    private readonly List<Layout> _layouts = [];

    public string Title => _builder.DocumentInformation.Title;

    public Newspaper(int issue, string? author)
    {
        Issue = issue;

        _builder.DocumentInformation.Producer = "Totletheyn";
        _builder.DocumentInformation.Title = "Issue #" + issue;
        _builder.DocumentInformation.CreationDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
        _builder.DocumentInformation.Author = author;

        Layout.Builder = _builder;

        Layout.PathResolver.Base = "Assets/";
        Layout.AddFont("Default", "fonts/NoticiaText-Regular.ttf");
        Layout.AddFont("Jaini", "fonts/Jaini-Regular.ttf");
        Layout.AddFont("NoticiaText", "fonts/NoticiaText-Regular.ttf");

        LayoutLoader.AddDataSource<WeatherDataSource>();
        LayoutLoader.AddDataSource<XkcdDataSource>();
        LayoutLoader.AddDataSource<NasaDataSource>();
        LayoutLoader.AddDataSource<JokeDataSource>();
        LayoutLoader.AddDataSource<ComicDataSource>();
        LayoutLoader.AddDataSource<TiobeDataSource>();
        LayoutLoader.AddDataSource<CryptoDataSource>();

        var coverLayout = LayoutLoader.Load("layouts/cover.xml");

        _layouts.Add(coverLayout);
    }

    private static void AddBookmarks(params Layout[] layouts)
    {
        var nodes = new List<DocumentBookmarkNode>();

        foreach (var layout in layouts)
        {
            nodes.Add(new DocumentBookmarkNode(layout.Name, 0,
                new ExplicitDestination(layout.Page!.PageNumber, ExplicitDestinationType.FitPage,
                    ExplicitDestinationCoordinates.Empty),
                [])
            );
        }

        Layout.Builder.Bookmarks = new(nodes);
    }

    public byte[] Render()
    {
        AddNewsToCover();

        foreach (var layout in _layouts)
        {
            layout.Apply();
        }

        AddBookmarks(_layouts.ToArray());

        return _builder.Build();
    }

    private void AddNewsToCover()
    {
        var coverLayout = _layouts[0];
        var articles = coverLayout.FindDescendantNodes("article").ToArray();

        var articleIndex = 0;
        var totalArticles = articles.Length;
        var feedQueue = new Queue<Feed>(Feeds);

        while (articleIndex < totalArticles && feedQueue.Count > 0)
        {
            var feed = feedQueue.Dequeue();

            foreach (var item in feed.Items)
            {
                if (articleIndex >= totalArticles)
                    break;

                var titleNode = articles[articleIndex].FindNode<TextNode>("title");
                if(titleNode is not null)
                    titleNode.Text = item.Title;

                var summaryNode = articles[articleIndex].FindNode<TextNode>("summary")!;
                if(titleNode is not null)
                    summaryNode.Text = item.Content;
                articleIndex++;
            }

            if (feed.Items.Any())
                feedQueue.Enqueue(feed);
        }
    }

    public void AddFeed(Feed feed)
    {
        Feeds.Add(feed);

        var layout = LayoutLoader.Load("layouts/content.xml", feed.Title);

        var articles = layout.FindDescendantNodes("article").ToArray();

        _layouts.Add(layout);
    }
}