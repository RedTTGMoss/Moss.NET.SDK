using System.Text.Json.Serialization;
using File = Moss.NET.Sdk.FFI.File;

namespace Moss.NET.Sdk;

internal class ExtensionInfo
{
    [JsonPropertyName("files")] public List<File> Files { get; internal set; } = null!;
}