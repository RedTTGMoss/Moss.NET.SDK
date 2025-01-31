using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk;

public readonly struct ExtensionInfo(List<File> files)
{
    [JsonPropertyName("files")]
    public List<File> Files { get; } = files;
}
