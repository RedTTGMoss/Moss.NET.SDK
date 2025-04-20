using System;
using System.Xml.Linq;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources;

public class JokeDataSource : IDataSource
{
    const string URL = "https://icanhazdadjoke.com/";
    public string Name => "joke";

    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (node is not ContainerNode container)
        {
            throw new ArgumentException("node is not a ContainerNode");
        }

        var template = new RestTemplate();
        template.Headers.Add("Accept", "text/plain");

        var content = template.GetString(URL).Replace('\r', '\0').Replace("\n", "-");

        container.Title = "Joke";

        var contentTextNode = node.ParentLayout.CreateTextNode(content, "jokeContent");
        contentTextNode.FontSize = 10;
        contentTextNode.FontFamily = "NoticiaText";
        contentTextNode.MarginTop = 5;
        contentTextNode.AutoSize = true;

        container.Content.Add(contentTextNode);
    }
}