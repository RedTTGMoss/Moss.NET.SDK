using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk;

public class ExtensionInfo
{
    [JsonPropertyName("files")] internal List<File> Files { get; set; }
}