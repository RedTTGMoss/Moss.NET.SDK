using System;
using System.Xml.Linq;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using Totletheyn.Core.RSS;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources;

public class XkcdDataSource : IDataSource
{
    const string URL = "https://xkcd.com/rss.xml";
    public string Name => "xkcd";

    private Feed feed;
    public XkcdDataSource()
    {
        feed = FeedReader.Read(URL);
    }

    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (node is not ContainerNode container)
        {
            throw new ArgumentException("node is not a ContainerNode");
        }

        container.Copyright = "xkcd";

        var imgHtml = feed.Items[0].Description;
        var doc = XDocument.Parse(imgHtml);

        var img = node.ParentLayout.CreateImageNode(doc.Root!.Attribute("src")!.Value);
        img.Width = 300;
        img.Height = 150;

        container.Width = img.Width;

        container.Content.Add(img);
    }
}