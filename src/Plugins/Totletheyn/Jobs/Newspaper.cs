using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core.RSS;
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

        builder.Bookmarks = new([
            new DocumentBookmarkNode("Cover", 0, new ExplicitDestination(1, ExplicitDestinationType.FitPage, ExplicitDestinationCoordinates.Empty), [])
        ]);
        builder.DocumentInformation.Producer = "Totletheyn";
        builder.DocumentInformation.Title = "Issue #" + issue;
        builder.DocumentInformation.CreationDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

        Layout.Builder = builder;

        //Layout.AddFont("default", Defaults.GetDefaultValue<string>("CUSTOM_FONT"));
        Layout.AddFont("Jaini", "extension/Assets/fonts/Jaini-Regular.ttf");
        Layout.AddFont("NoticiaText", "extension/Assets/fonts/NoticiaText-Regular.ttf");
    }

    private Base64 Render()
    {
        var coverLayout = LayoutLoader.LoadLayoutFromXml(File.ReadAllText("extension/Assets/cover.xml"));
        coverLayout.FindNode<TextNode>("header headerInfo date")!.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

        coverLayout.Apply();

        return builder.Build();
    }

    public PdfNotebook CreateNotebook(string folder)
    {
        return new PdfNotebook("Issue #" + _issue, Render(), folder);
    }
}