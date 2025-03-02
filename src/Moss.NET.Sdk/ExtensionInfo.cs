using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk;

public class ExtensionInfo
{
    [JsonPropertyName("files")] public List<File> Files { get; internal set; }
}