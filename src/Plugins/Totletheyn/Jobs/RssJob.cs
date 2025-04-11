using System;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Scheduler;

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
        /*
        _lastUpdated = DateTimeOffset.UtcNow;
        _logger.Info($"Running RssJob with '{Options.feeds.Length}' feeds.");

        var output = new Base64();
        var writer = new EpubWriter();

        writer.SetTitle(_lastUpdated.ToString());
        writer.AddAuthor(Options.author ?? "Totletheyn");

        writer.AddChapter("Cover", File.ReadAllText("extension/Assets/cover_template.html"));

        writer.AddFile("fonts.css", File.ReadAllBytes("extension/Assets/fonts.css"), EpubContentType.Css);
        writer.AddFile("style.css", File.ReadAllBytes("extension/Assets/style.css"), EpubContentType.Css);

        writer.AddFile("fonts/Jaini-Regular.ttf", File.ReadAllBytes("extension/Assets/fonts/Jaini-Regular.ttf"),
            EpubContentType.FontTruetype);
        writer.AddFile("fonts/NoticiaText-Regular.ttf",
            File.ReadAllBytes("extension/Assets/fonts/NoticiaText-Regular.ttf"), EpubContentType.FontTruetype);

        foreach (string url in Options.feeds)
        {
            var feed = FeedReader.Read(url);

            foreach (var item in feed.Items)
                if (item.PublishingDate > _lastUpdated)
                    writer.AddChapter(item.Title, item.Description);
        }

        writer.Write(output);

        _logger.Info($"Saving generated feed to {Options.folder}");
        var notebook = new EpubNotebook(_lastUpdated.ToString(), output, Options.folder);
        notebook.Upload();
        */
    }

    public override void Shutdown()
    {
        Data = _lastUpdated;
    }
}