using System;
using System.Linq;
using System.Xml.Linq;
using HtmlAgilityPack;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources;

public class ComicDataSource : IDataSource
{
    public string Name => "comic";
    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (node is not ContainerNode container)
        {
            throw new ArgumentException("node is not a ContainerNode");
        }

        var series = "garfield";
        var template = new RestTemplate();
        var content = template.GetString($"https://www.gocomics.com/{series}/");

        var img = node.ParentLayout.CreateImageNode(GetImageUri(content));
        img.Width = 315;
        img.Height = 200;

        container.Content.Add(img);
        container.Copyright = "GoComics.com";
        container.Title = "Comic";
        container.Width = img.Width;
    }

    private static string GetImageUri(string source)
    {
        var document = new HtmlDocument();

        document.LoadHtml(source);

        var imageNode = document.DocumentNode.SelectNodes("//img")!
            .Where(_ => _.GetAttributeValue("class", "").StartsWith("Comic_comic__image"))
            .Select(x => x.GetAttributeValue("src", ""));

        return imageNode.First().Replace("quality=85", "quality=100");
    }
}