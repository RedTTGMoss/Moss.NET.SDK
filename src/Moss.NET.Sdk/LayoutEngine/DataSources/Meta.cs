using System.Xml.Linq;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.DataSources;

public class Meta : IDataSource
{
    public string Name => "meta";
    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        var metaAttribute = element.Attribute("meta")?.Value ?? string.Empty;

        if (string.IsNullOrEmpty(metaAttribute))
        {
            throw new ArgumentException("meta cannot be null or empty");
        }

        if (node is not TextNode textNode)
        {
            throw new ArgumentException("meta information can only be applied to text nodes");
        }

        textNode.Text = metaAttribute switch
        {
            "author" => Layout.Builder.DocumentInformation.Author!,
            "title" => Layout.Builder.DocumentInformation.Title!,
            "creation-date" => Layout.Builder.DocumentInformation.CreationDate!,
            "producer" => Layout.Builder.DocumentInformation.Producer,
            "page" => "Page " + (Layout.Builder.Pages.Count - 1),
            _ => throw new ArgumentException($"meta attribute '{metaAttribute}' not found")
        };
    }
}