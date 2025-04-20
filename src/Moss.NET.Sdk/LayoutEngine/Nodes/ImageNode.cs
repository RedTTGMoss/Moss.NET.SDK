using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public enum ImageFormat {Png, Jpeg}

public class ImageNode(YogaConfig config) : YogaNode(config)
{
    public required string Src { get; set; }
    public bool AutoSize { get; set; }

    private byte[] _data;
    private ImageFormat ImageFormat;

    public override void ReCalculate(PdfPageBuilder page)
    {
        base.ReCalculate(page);
        _data = LoadImage();

        DetectImageFormat();

        if (AutoSize)
        {
            AdjustSize();
        }
    }

    private void DetectImageFormat()
    {
        if (GetMagicNumber(4).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47 })) // PNG
        {
            ImageFormat = ImageFormat.Png;
        }
        else if (GetMagicNumber(2).SequenceEqual(new byte[] { 0xFF, 0xD8 })) // JPEG
        {
            ImageFormat = ImageFormat.Jpeg;
        }
        else
        {
            throw new ArgumentException($"invalid image format '{Src}'");
        }
    }

    private void AdjustSize()
    {

        (int width, int height) dimension;
        if ( Src.EndsWith(".png"))
        {
            dimension = ReadPngDimension();
        }
        else if (Src.EndsWith(".jpg") || Src.EndsWith(".jpeg"))
        {
            dimension = ReadJpegDimension();
        }
        else
        {
            throw new ArgumentException($"invalid image format '{Src}'");
        }

        Width = dimension.width;
        Height = dimension.height;
    }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        if (Display == YogaDisplay.None)
        {
            return;
        }

        base.Draw(page, absoluteX, absoluteY);

        var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
        var rect = new PdfRectangle(boxPos, new PdfPoint(boxPos.X + LayoutWidth, boxPos.Y + LayoutHeight));

        PdfPageBuilder.AddedImage img;

        if (ImageFormat == ImageFormat.Png)
        {
            img = page.AddPng(new MemoryStream(_data), rect);
        }
        else if (ImageFormat == ImageFormat.Jpeg)
        {
            img = page.AddJpeg(new MemoryStream(_data), rect);
        }
        else
        {
            throw new ArgumentException($"invalid image format '{Src}'");
        }

        page.AddImage(img, rect);
    }

    private byte[] LoadImage()
    {
        byte[] imageBytes;

        if (Src.StartsWith("http") || Src.StartsWith("https"))
        {
            var template = new RestTemplate();
            imageBytes = template.GetBytes(Src);
        }
        else
        {
            imageBytes = LayoutEngine.Layout.PathResolver.ReadBytes(Src);
        }

        return imageBytes;
    }

    internal override void SetAttribute(string name, string value)
    {
        switch (name)
        {
            case "src":
                Src = value;
                break;
            case "autosize":
                AutoSize = value == "true";
                break;
        }
    }

    private (int width, int height) ReadPngDimension()
    {
        var stream = new MemoryStream(_data);

        stream.Seek(16, SeekOrigin.Begin);
        var buffer = new byte[4];
        stream.ReadExactly(buffer, 0, 4);
        var width = BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);

        stream.Seek(20, SeekOrigin.Begin);
        stream.ReadExactly(buffer, 0, 4);
        var height = BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);

        return (width, height);
    }

    private (int width, int height) ReadJpegDimension()
    {
        using var stream = new MemoryStream(_data);
        var b1 = stream.ReadByte();
        var b2 = stream.ReadByte();

        if (b1 != 0xFF || b2 != 0xD8)
            throw new InvalidOperationException("invalid JPEG.");

        while (stream.Position < stream.Length)
        {
            int markerStart;
            do
            {
                markerStart = stream.ReadByte();
            }
            while (markerStart != 0xFF && stream.Position < stream.Length);

            int markerType;
            do
            {
                markerType = stream.ReadByte();
            }
            while (markerType == 0xFF && stream.Position < stream.Length);

            if (markerType >= 0xC0 && markerType <= 0xCF &&
                markerType != 0xC4 && markerType != 0xC8 && markerType != 0xCC)
            {
                stream.ReadByte(); // High
                stream.ReadByte(); // Low

                stream.ReadByte(); // skip Precision

                var height = (stream.ReadByte() << 8) + stream.ReadByte();
                var width = (stream.ReadByte() << 8) + stream.ReadByte();

                return (width, height);
            }
            else if (markerType is 0xD9 or 0xDA)
            {
                break;
            }
            else
            {
                var len = (stream.ReadByte() << 8) + stream.ReadByte();
                if (len < 2)
                    throw new InvalidOperationException("Invalid JPEG-Segment.");
                stream.Seek(len - 2, SeekOrigin.Current);
            }
        }

        throw new InvalidOperationException("SOF-Marker not found");
    }

    private byte[] GetMagicNumber(int count)
    {
        var buffer = new byte[count];
        using var stream = new MemoryStream(_data);
        stream.ReadExactly(buffer, 0, buffer.Length);
        return buffer;
    }
}