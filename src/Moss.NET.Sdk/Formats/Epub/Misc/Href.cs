using System;

#pragma warning disable CS8618, CS9264

namespace Moss.NET.Sdk.Formats.Epub.Misc;

internal class Href
{
    public readonly string HashLocation;
    public readonly string? Path;

    public Href(string? href)
    {
        if (string.IsNullOrWhiteSpace(href)) throw new ArgumentNullException(nameof(href));

        var contentSourceAnchorCharIndex = href.IndexOf('#');
        if (contentSourceAnchorCharIndex == -1)
        {
            Path = href;
        }
        else
        {
            Path = href.Substring(0, contentSourceAnchorCharIndex);
            HashLocation = href.Substring(contentSourceAnchorCharIndex + 1);
        }
    }
}