using System.Linq;
using System.Xml.Linq;
using HtmlAgilityPack;
using Moss.NET.Sdk.LayoutEngine;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources;

public class ComicDataSource : IDataSource
{
    public string Name => "comic";
    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        var series = "garfield";
        var template = new RestTemplate();
        var content = template.GetString($"https://www.gocomics.com/{series}/");

        var img = node.ParentLayout.CreateImageNode(GetImageUri(content));
        img.Width = 300;
        img.Height = 150;

        node.Add(img);

        node.Margin = 10;

        var text = node.ParentLayout.CreateTextNode("© GoComics");
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

    private static string GetImageUri(string source)
    {
        var document = new HtmlDocument();
        document.LoadHtml(source);

        var imageNode = document.DocumentNode.SelectNodes("//img")!
            .Where(_ => _.GetAttributeValue("class", "").StartsWith("Comic_comic__image"))
            .Select(x => x.GetAttributeValue("src", ""));

        return imageNode.FirstOrDefault();
    }
}