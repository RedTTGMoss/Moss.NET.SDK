using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class ImageNode(YogaConfig config) : YogaNode(config)
{
    public required Uri Src { get; set; }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        if (Display == YogaDisplay.None)
        {
            return;
        }

        base.Draw(page, absoluteX, absoluteY);

        byte[] imageBytes = null;
        var url = Src.ToString();
        if (Src is { IsAbsoluteUri: true, Scheme: "http" or "https" })
        {
            //Todo: convert to moss rest template
            var template = new RestTemplate();
            imageBytes = template.GetBytes(url);
        }

        if (Src.IsFile)
        {
            imageBytes = File.ReadAllBytes(url.Replace("file://", ""));
        }

        var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
        var rect = new PdfRectangle(boxPos, new PdfPoint(boxPos.X + LayoutWidth, boxPos.Y + LayoutHeight));

        PdfPageBuilder.AddedImage img;

        if (url.EndsWith(".png"))
        {
            img = page.AddPng(new MemoryStream(imageBytes), rect);
        }
        else if (url.EndsWith(".jpg") ||
                 url.EndsWith(".jpeg"))
        {
            img = page.AddJpeg(new MemoryStream(imageBytes), rect);
        }
        else
        {
            throw new ArgumentException($"invalid image format '{url}'");
        }

        page.AddImage(img, rect);
    }

    internal override void SetAttribute(string name, string value)
    {
        if (name == "src")
        {
            Src = new Uri(value);
        }
    }
}