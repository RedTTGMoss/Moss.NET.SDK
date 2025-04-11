using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Moss.NET.Sdk.Formats.Epub.Format;

public class OcfDocument
{
    private OcfRootFile rootFile;
    public IList<OcfRootFile> RootFiles { get; internal set; } = new List<OcfRootFile>();

    public string? RootFilePath => rootFile?.FullPath ??
                                   (rootFile = RootFiles.FirstOrDefault(e => e.MediaType == Constants.OcfMediaType))
                                   ?.FullPath;
}

public class OcfRootFile
{
    public string? FullPath { get; internal set; }
    public string MediaType { get; internal set; }

    internal static class Attributes
    {
        public static readonly XName FullPath = "full-path";
        public static readonly XName MediaType = "media-type";
    }
}