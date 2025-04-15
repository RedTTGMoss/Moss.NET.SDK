using System.Globalization;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class HorizontalLineNode(YogaConfig config) : YogaNode(config)
{
    public double LineThickness { get; set; } = 1.0;
    public Color LineColor { get; set; } = Colors.Black;

    public override void ReCalculate(PdfPageBuilder page)
    {
        Height = LineThickness;
        Background = LineColor;
    }

    protected override void SetAttribute(string name, string value)
    {
        if (name == "lineColor")
        {
            LineColor = Colors.FromName(value);
        }
        else if (name == "thickness")
        {
            LineThickness = double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}