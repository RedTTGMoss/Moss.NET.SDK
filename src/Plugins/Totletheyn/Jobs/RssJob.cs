﻿using System;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Formats.Core;
using Moss.NET.Sdk.Scheduler;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core.RSS;

namespace Totletheyn.Jobs;

public class RssJob : Job
{
    private readonly LoggerInstance _logger = Log.GetLogger<RssJob>();
    private DateTimeOffset _lastUpdated;

    public override void Init()
    {
        Data ??= DateTimeOffset.UtcNow;

        _lastUpdated = (DateTimeOffset)Data;
    }

    public override void Run()
    {
        _lastUpdated = DateTimeOffset.UtcNow;
        _logger.Info($"Running WikiJob with language '{Options.language}'");

        var output = new Base64();
        var writer = new EpubWriter();

        writer.SetTitle(_lastUpdated.ToString());
        writer.AddAuthor(Options.author ?? "Totletheyn");

        writer.AddChapter("Chapter 1", "This is the first chapter");
        writer.AddChapter("Chapter 2", "This is the second chapter");

        foreach (string feed in Options.feeds)
        {
            var reader = FeedReader.Read(feed);

            foreach (var item in reader.Items)
            {
                if (item.PublishingDate > _lastUpdated)
                {
                    writer.AddChapter(item.Title, item.Description);
                }
            }
        }

        writer.Write(output);

        _logger.Info($"Saving generated feed to {Options.folder}");
        var notebook = new EpubNotebook(_lastUpdated.ToString(), output, Options.folder);
        notebook.Upload();
    }

    public override void Shutdown()
    {
        Data = _lastUpdated;
    }
}