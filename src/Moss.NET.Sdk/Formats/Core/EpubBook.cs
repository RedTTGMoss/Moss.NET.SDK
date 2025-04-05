using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moss.NET.Sdk.Formats.Core.Format;
using Moss.NET.Sdk.Formats.Core.Misc;
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.Formats.Core;

public class EpubBook
{
    internal const string AuthorsSeparator = ", ";

    /// <summary>
    /// Read-only raw epub format structures.
    /// </summary>
    public EpubFormat Format { get; internal set; }

    public string? Title => Format.Opf.Metadata.Titles.FirstOrDefault();

    public IEnumerable<string> Authors => Format.Opf.Metadata.Creators.Select(creator => creator.Text);

    /// <summary>
    /// All files within the EPUB.
    /// </summary>
    public EpubResources Resources { get; internal set; } = null!;

    /// <summary>
    /// EPUB format specific resources.
    /// </summary>
    public EpubSpecialResources SpecialResources { get; internal set; }

    public byte[]? CoverImage { get; internal set; }

    public IList<EpubChapter> TableOfContents { get; internal set; }

    public string ToPlainText()
    {
        var builder = new StringBuilder();
        foreach (var html in SpecialResources.HtmlInReadingOrder)
        {
            builder.Append(Html.GetContentAsPlainText(html.TextContent));
            builder.Append('\n');
        }
        return builder.ToString().Trim();
    }
}

public class EpubChapter
{
    public string Id { get; set; }
    public string? AbsolutePath { get; set; }
    public string? RelativePath { get; set; }
    public string HashLocation { get; set; }
    public string Title { get; set; }

    public EpubChapter? Parent { get; set; }
    public EpubChapter? Previous { get; set; }
    public EpubChapter Next { get; set; }
    public IList<EpubChapter> SubChapters { get; set; } = new List<EpubChapter>();

    public override string ToString()
    {
        return $"Title: {Title}, Subchapter count: {SubChapters.Count}";
    }
}

public class EpubResources
{
    public IList<EpubTextFile> Html { get; } = new List<EpubTextFile>();
    public IList<EpubTextFile> Css { get; } = new List<EpubTextFile>();
    public IList<EpubByteFile> Images { get; } = new List<EpubByteFile>();
    public IList<EpubByteFile> Fonts { get; } = new List<EpubByteFile>();
    public IList<EpubFile> Other { get; internal set; } = new List<EpubFile>();

    /// <summary>
    /// This is a concatination of all the resources files in the epub: html, css, images, etc.
    /// </summary>
    public IList<EpubFile> All { get; } = new List<EpubFile>();
}

public class EpubSpecialResources
{
    public EpubTextFile Ocf { get; internal set; }
    public EpubTextFile Opf { get; internal set; }
    public IList<EpubTextFile> HtmlInReadingOrder { get; internal init; } = new List<EpubTextFile>();
}

public abstract class EpubFile
{
    public string? AbsolutePath { get; init; }
    public string? Href { get; init; }
    public EpubContentType ContentType { get; init; }
    public string MimeType { get; set; }
    public byte[]? Content { get; set; }
}

public class EpubByteFile : EpubFile
{
    internal EpubTextFile ToTextFile()
    {
        return new EpubTextFile
        {
            Content = Content,
            ContentType = ContentType,
            AbsolutePath = AbsolutePath,
            Href = Href,
            MimeType = MimeType
        };
    }
}

public class EpubTextFile : EpubFile
{
    public string TextContent {
        get => Constants.DefaultEncoding!.GetString(Content, 0, Content.Length);
        init => Content = Constants.DefaultEncoding!.GetBytes(value);
    }
}