using System.Collections.Generic;
using System.Text.Json.Serialization;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

internal class ExtensionInfo
{
    [JsonPropertyName("files")] public List<File> Files { get; internal set; }= null!;
}