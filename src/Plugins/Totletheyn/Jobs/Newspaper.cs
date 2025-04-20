using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.DataSources;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core.RSS;
using Totletheyn.DataSources;
using Totletheyn.DataSources.Weather;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.Jobs;

class Newspaper
{
    public List<Feed> Feeds { get; } = [];
    public List<FeedItem> Items { get; } = [];

    private readonly PdfDocumentBuilder builder = new();
    private readonly int _issue;
    private readonly string _author;

    public Newspaper(int issue, string? author)
    {
        _issue = issue;
        _author = author;

        builder.DocumentInformation.Producer = "Totletheyn";
        builder.DocumentInformation.Title = "Issue #" + issue;
        builder.DocumentInformation.CreationDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
        builder.DocumentInformation.Author = author;

        Layout.Builder = builder;

        Layout.AddFont("Default", "fonts/NoticiaText-Regular.ttf");
        Layout.AddFont("Jaini", "fonts/Jaini-Regular.ttf");
        Layout.AddFont("NoticiaText", "fonts/NoticiaText-Regular.ttf");

        LayoutLoader.AddDataSource<Meta>();
        LayoutLoader.AddDataSource<WeatherDataSource>();
        LayoutLoader.AddDataSource<XkcdDataSource>();
        LayoutLoader.AddDataSource<NasaDataSource>();
        LayoutLoader.AddDataSource<JokeDataSource>();
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

    private Base64 Render()
    {
        var coverLayout = LayoutLoader.Load("layouts/cover.xml");
        //coverLayout.EnableDebugLines();
        coverLayout.Apply();

        var contentLayout = LayoutLoader.Load("layouts/content.xml");
        contentLayout.Name = "Page 1";
        contentLayout.Apply();

        AddBookmarks(coverLayout, contentLayout);

        return builder.Build();
    }

    public PdfNotebook CreateNotebook(string folder)
    {
        return new PdfNotebook("Issue #" + _issue, Render(), folder);
    }
}