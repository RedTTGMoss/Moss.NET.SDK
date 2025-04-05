using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Extism;
using Totletheyn.Core.RSS.Feeds.MediaRSS;

namespace Totletheyn.Core.RSS;

/// <summary>
///     static class with helper functions
/// </summary>
public static class Helpers
{
    private const string ACCEPT_HEADER_NAME = "Accept";

    private const string ACCEPT_HEADER_VALUE =
        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

    private const string USER_AGENT_NAME = "User-Agent";
    private const string USER_AGENT_VALUE = "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.0";


    /// <summary>
    ///     Download the content from an url
    /// </summary>
    /// <param name="url">correct url</param>
    /// <returns>Content as string</returns>
    [Obsolete("Use the DownloadAsync method")]
    public static string Download(string url)
    {
        return DownloadAsync(url).GetAwaiter().GetResult();
    }

    /// <summary>
    ///     Download the content from an url
    /// </summary>
    /// <param name="url">correct url</param>
    /// <param name="userAgent">override built-in user-agent header</param>
    /// <returns>Content as byte array</returns>
    public static Task<byte[]> DownloadBytesAsync(string url)
    {
        url = WebUtility.UrlDecode(url);

        var request = new HttpRequest(url)
        {
            Method = HttpMethod.GET
        };
        request.Headers.Add(USER_AGENT_NAME, USER_AGENT_NAME);
        request.Headers.Add(ACCEPT_HEADER_NAME, ACCEPT_HEADER_VALUE);

        var response = Pdk.SendRequest(request);

        if (response.Status != 200) throw new Exception(response.Status.ToString());

        return Task.FromResult(response.Body.ReadBytes());
    }

    /// <summary>
    ///     Download the content from an url
    /// </summary>
    /// <param name="url">correct url</param>
    /// <param name="autoRedirect">autoredirect if page is moved permanently</param>
    /// <returns>Content as byte array</returns>
    public static Task<byte[]> DownloadBytesAsync(string url, bool autoRedirect = true)
    {
        return DownloadBytesAsync(url, autoRedirect);
    }

    /// <summary>
    ///     Download the content from an url and returns it as utf8 encoded string.
    ///     Preferred way is to use <see cref="DownloadBytesAsync(string, bool)" /> because it works
    ///     better with encoding.
    /// </summary>
    /// <param name="url">correct url</param>
    /// <param name="cancellationToken">token to cancel operation</param>
    /// <param name="autoRedirect">autoredirect if page is moved permanently</param>
    /// <returns>Content as string</returns>
    public static async Task<string> DownloadAsync(string url, CancellationToken cancellationToken,
        bool autoRedirect = true)
    {
        var content = await DownloadBytesAsync(url, autoRedirect).ConfigureAwait(false);
        return Encoding.UTF8.GetString(content);
    }

    /// <summary>
    ///     Download the content from an url and returns it as utf8 encoded string.
    ///     Preferred way is to use <see cref="DownloadBytesAsync(string, bool)" /> because it works
    ///     better with encoding.
    /// </summary>
    /// <param name="url">correct url</param>
    /// <param name="autoRedirect">autoredirect if page is moved permanently</param>
    /// <returns>Content as string</returns>
    public static Task<string> DownloadAsync(string url, bool autoRedirect = true)
    {
        return DownloadAsync(url, CancellationToken.None, autoRedirect);
    }

    /// <summary>
    ///     Tries to parse the string as datetime and returns null if it fails
    /// </summary>
    /// <param name="datetime">datetime as string</param>
    /// <param name="cultureInfo">The cultureInfo for parsing</param>
    /// <returns>datetime or null</returns>
    public static DateTime? TryParseDateTime(string datetime, CultureInfo cultureInfo = null)
    {
        if (string.IsNullOrWhiteSpace(datetime))
            return null;

        var dateTimeFormat = cultureInfo?.DateTimeFormat ?? DateTimeFormatInfo.CurrentInfo;
        var parseSuccess = DateTimeOffset.TryParse(datetime, dateTimeFormat, DateTimeStyles.None, out var dt);

        if (!parseSuccess)
        {
            // Do, 22 Dez 2016 17:36:00 +0000
            // note - tried ParseExact with diff formats like "ddd, dd MMM yyyy hh:mm:ss K"
            if (datetime.Contains(","))
            {
                var pos = datetime.IndexOf(',') + 1;
                var newdtstring = datetime.Substring(pos).Trim();

                parseSuccess = DateTimeOffset.TryParse(newdtstring, dateTimeFormat, DateTimeStyles.None, out dt);
            }

            if (!parseSuccess)
            {
                var newdtstring = datetime.Substring(0, datetime.LastIndexOf(" ")).Trim();

                parseSuccess = DateTimeOffset.TryParse(newdtstring, dateTimeFormat, DateTimeStyles.AssumeUniversal,
                    out dt);
            }

            if (!parseSuccess)
            {
                var newdtstring = datetime.Substring(0, datetime.LastIndexOf(" ")).Trim();

                parseSuccess = DateTimeOffset.TryParse(newdtstring, dateTimeFormat, DateTimeStyles.None,
                    out dt);
            }
        }

        if (!parseSuccess)
            return null;

        return dt.UtcDateTime;
    }

    /// <summary>
    ///     Tries to parse the string as int and returns null if it fails
    /// </summary>
    /// <param name="input">int as string</param>
    /// <returns>integer or null</returns>
    public static int? TryParseInt(string input)
    {
        if (!int.TryParse(input, out var tmp))
            return null;
        return tmp;
    }

    /// <summary>
    ///     Tries to parse a string and returns the media type
    /// </summary>
    /// <param name="medium">media type as string</param>
    /// <returns>
    ///     <see cref="Medium" />
    /// </returns>
    public static Medium TryParseMedium(string medium)
    {
        if (string.IsNullOrEmpty(medium)) return Medium.Unknown;

        switch (medium.ToLower())
        {
            case "image":
                return Medium.Image;
            case "audio":
                return Medium.Audio;
            case "video":
                return Medium.Video;
            case "document":
                return Medium.Document;
            case "executable":
                return Medium.Executable;
            default:
                return Medium.Unknown;
        }
    }

    /// <summary>
    ///     Tries to parse the string as int and returns null if it fails
    /// </summary>
    /// <param name="input">int as string</param>
    /// <returns>integer or null</returns>
    public static bool? TryParseBool(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            input = input.ToLower();

            if (input == "true") return true;

            if (input == "false") return false;
        }

        return null;
    }

    /// <summary>
    ///     Returns a HtmlFeedLink object from a linktag (link href="" type="")
    ///     only support application/rss and application/atom as type
    ///     if type is not supported, null is returned
    /// </summary>
    /// <param name="input">
    ///     link tag, e.g. &lt;link rel="alternate" type="application/rss+xml" title="codehollow &gt; Feed"
    ///     href="https://codehollow.com/feed/" /&gt;
    /// </param>
    /// <returns>Parsed HtmlFeedLink</returns>
    public static HtmlFeedLink GetFeedLinkFromLinkTag(string input)
    {
        var linkTag = input.HtmlDecode();
        var type = GetAttributeFromLinkTag("type", linkTag).ToLower();

        if (!type.Contains("application/rss") && !type.Contains("application/atom"))
            return null;

        var hfl = new HtmlFeedLink();
        var title = GetAttributeFromLinkTag("title", linkTag);
        var href = GetAttributeFromLinkTag("href", linkTag);
        hfl.Title = title;
        hfl.Url = href;
        hfl.FeedType = type.Contains("rss") ? FeedType.Rss : FeedType.Atom;
        return hfl;
    }

    /// <summary>
    ///     Parses RSS links from html page and returns all links
    /// </summary>
    /// <param name="htmlContent">the content of the html page</param>
    /// <returns>all RSS/feed links</returns>
    public static IEnumerable<HtmlFeedLink> ParseFeedUrlsFromHtml(string htmlContent)
    {
        // sample link:
        // <link rel="alternate" type="application/rss+xml" title="Microsoft Bot Framework Blog" href="http://blog.botframework.com/feed.xml">
        // <link rel="alternate" type="application/atom+xml" title="Aktuelle News von heise online" href="https://www.heise.de/newsticker/heise-atom.xml">

        var rex = new Regex("<link[^>]*rel=\"alternate\"[^>]*>", RegexOptions.Singleline);

        var result = new List<HtmlFeedLink>();

        foreach (Match m in rex.Matches(htmlContent))
        {
            var hfl = GetFeedLinkFromLinkTag(m.Value);
            if (hfl != null)
                result.Add(hfl);
        }

        return result;
    }

    /// <summary>
    ///     reads an attribute from an html tag
    /// </summary>
    /// <param name="attribute">name of the attribute, e.g. title</param>
    /// <param name="htmlTag">the html tag, e.g. &lt;link title="my title"&gt;</param>
    /// <returns>the value of the attribute, e.g. my title</returns>
    private static string GetAttributeFromLinkTag(string attribute, string htmlTag)
    {
        var res = Regex.Match(htmlTag, attribute + "\\s*=\\s*\"(?<val>[^\"]*)\"",
            RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);

        if (res.Groups.Count > 1)
            return res.Groups[1].Value;
        return string.Empty;
    }
}