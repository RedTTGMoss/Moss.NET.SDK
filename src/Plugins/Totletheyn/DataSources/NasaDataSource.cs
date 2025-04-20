using System;
using System.Xml.Linq;
using HtmlAgilityPack;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using Totletheyn.Core.RSS;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources;

public class NasaDataSource : IDataSource
{
    const string URL = "https://apod.nasa.gov/apod.rss";
    public string Name => "nasa";

    private Feed feed;
    public NasaDataSource()
    {
        feed = FeedReader.Read(URL);
    }

    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (node is not ContainerNode container)
        {
            throw new ArgumentException("node is not a ContainerNode");
        }

        var template = new RestTemplate();
        var link = feed.Items[0].Link;

        var doc = new HtmlDocument();
        doc.LoadHtml(template.GetString(link));

        var a = doc.DocumentNode.SelectSingleNode("//html//body//center/p//a//img")!;

        var img = node.ParentLayout.CreateImageNode($"https://apod.nasa.gov/apod/{a.Attributes["src"].Value}");
        img.Width = 300;
        img.Height = 150;

        container.Copyright = "NASA";
        container.Width = img.Width;

        container.Content.Add(img);
    }
}