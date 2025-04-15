using System;
using System.IO;
using System.Net;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class ImageNode : YogaNode
{
    public required Uri Src { get; set; }

    public ImageNode(YogaConfig config) : base(config)
    {
    }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        if (Display == YogaDisplay.None)
        {
            return;
        }

        base.Draw(page, absoluteX, absoluteY);

        byte[] imageBytes = null;
        if (Src is { IsAbsoluteUri: true, Scheme: "http" or "https" })
        {
            var template = new RestTemplate();
            imageBytes = template.Exchange(Src.ToString()).Body.ReadBytes();
        }

        if (Src.IsFile)
        {
            imageBytes = File.ReadAllBytes(Src.ToString().Replace("file://", ""));
        }

        var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
        var rect = new PdfRectangle(boxPos, new PdfPoint(boxPos.X + LayoutWidth, boxPos.Y + LayoutHeight));
        var img = page.AddPng(new MemoryStream(imageBytes), rect);

        page.AddImage(img, rect);
    }

    protected override void SetAttribute(string name, string value)
    {
        if (name == "src")
        {
            Src = new Uri(value);
        }
    }
}