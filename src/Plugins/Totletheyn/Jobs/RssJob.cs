﻿using System;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core.RSS;

namespace Totletheyn.Jobs;

public class RssJob : Job
{
    private static readonly LoggerInstance Logger = Log.GetLogger<RssJob>();
    private DateTimeOffset _lastUpdated;
    private int issue = 1;

    public override void Init()
    {
        if (Data is not null) issue = (int)Data;

        //_lastUpdated = (DateTimeOffset)Data;
    }

    public override void Run()
    {
        _lastUpdated = DateTimeOffset.UtcNow;
        Logger.Info($"Running RssJob with '{Options.feeds.Length}' feeds.");

        var newspaper = new Newspaper(issue, Options.author ?? "Totletheyn");

        foreach (string url in Options.feeds)
        {
            var feed = FeedReader.Read(url);
            newspaper.AddFeed(feed);
        }

        Logger.Info($"Saving generated feed to {Options.folder}");
        var notebook = new PdfNotebook(newspaper.Title, newspaper.Render());
        notebook.MoveTo(Options.folder);
        notebook.Upload();
    }

    public override void Shutdown()
    {
        Data = issue;
    }
}