using System.Xml.Linq;
using HtmlAgilityPack;
using Moss.NET.Sdk.LayoutEngine;
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
        var template = new RestTemplate();
        var link = feed.Items[0].Link;

        var doc = new HtmlDocument();
        doc.LoadHtml(template.GetString(link));

        var a = doc.DocumentNode.SelectSingleNode("//html//body//center/p//a//img")!;

        var img = node.ParentLayout.CreateImageNode($"https://apod.nasa.gov/apod/{a.Attributes["src"].Value}");
        img.Width = 300;
        img.Height = 150;

        node.Add(img);

        node.Margin = 10;

        var text = node.ParentLayout.CreateTextNode("© NASA");
        text.FontFamily = "NoticiaText";
        text.FontSize = 6;
        text.AlignSelf = YogaAlign.FlexEnd;
        text.AutoSize = true;
        text.MarginTop = 5;

        node.Add(text);

        node.FlexDirection = YogaFlexDirection.Column;
        node.Width = img.Width;
        node.Height = img.Height.Value!.Value + 10;
    }
}