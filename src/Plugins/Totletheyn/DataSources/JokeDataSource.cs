using System.Xml.Linq;
using Moss.NET.Sdk.LayoutEngine;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources;

public class JokeDataSource : IDataSource
{
    const string URL = "https://icanhazdadjoke.com/";
    public string Name => "joke";

    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        var template = new RestTemplate();
        template.Headers.Add("Accept", "text/plain");

        var content = template.GetString(URL);

        var jokeBox = node.ParentLayout.CreateNode("jokeBox");
        jokeBox.BorderColor = Colors.Black;
        jokeBox.Padding = 10;
        jokeBox.FlexDirection = YogaFlexDirection.Column;

        var titleNode = node.ParentLayout.CreateTextNode("Joke", "jokeTitle");
        titleNode.FontSize = 12;
        titleNode.FontFamily = "NoticiaText";
        titleNode.AlignSelf = YogaAlign.Center;
        titleNode.MarginBottom = 5;
        titleNode.AutoSize = true;

        var jokeTextNode = node.ParentLayout.CreateTextNode(content, "jokeContent");
        jokeTextNode.FontSize = 10;
        jokeTextNode.FontFamily = "NoticiaText";
        jokeTextNode.MarginTop = 5;
        jokeTextNode.AutoSize = true;

        jokeBox.Add(titleNode);
        jokeBox.Add(jokeTextNode);

        node.Add(jokeBox);
    }
}