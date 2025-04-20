using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class ImageNode(YogaConfig config) : YogaNode(config)
{
    public required string Src { get; set; }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        if (Display == YogaDisplay.None)
        {
            return;
        }

        base.Draw(page, absoluteX, absoluteY);

        byte[] imageBytes = null;

        if (Src.StartsWith("http") || Src.StartsWith("https"))
        {
            var template = new RestTemplate();
            imageBytes = template.GetBytes(Src);
        }
        else
        {
            imageBytes = LayoutEngine.Layout.PathResolver.ReadBytes(Src);
        }

        var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
        var rect = new PdfRectangle(boxPos, new PdfPoint(boxPos.X + LayoutWidth, boxPos.Y + LayoutHeight));

        PdfPageBuilder.AddedImage img = null;

        if (Src.EndsWith(".png"))
        {
            img = page.AddPng(new MemoryStream(imageBytes), rect);
        }
        else if (Src.EndsWith(".jpg") || Src.EndsWith(".jpeg"))
        {
            img = page.AddJpeg(new MemoryStream(imageBytes), rect);
        }
        else
        {
            throw new ArgumentException($"invalid image format '{Src}'");
        }

        page.AddImage(img, rect);
    }

    internal override void SetAttribute(string name, string value)
    {
        if (name == "src")
        {
            Src = value;
        }
    }
}