#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Moss.NET.Sdk.Formats.Core.Format;

public class EpubFormatPaths
{
    public string? OcfAbsolutePath { get; internal set; }
    public string? OpfAbsolutePath { get; internal set; }
    public string? NcxAbsolutePath { get; internal set; }
    public string? NavAbsolutePath { get; internal set; }
}

public class EpubFormat
{
    public EpubFormatPaths Paths { get; internal set; } = new();

    public OcfDocument Ocf { get; internal set; }
    public OpfDocument Opf { get; internal set; }
    public NcxDocument Ncx { get; internal set; }
    public NavDocument Nav { get; internal set; }
}