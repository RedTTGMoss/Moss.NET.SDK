using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.FFI;

[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(Color))]
[JsonSerializable(typeof(TextColor))]
[JsonSerializable(typeof(ContextButton))]
[JsonSerializable(typeof(List<ContextButton>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(List<ConfigGetD>))]
[JsonSerializable(typeof(List<ConfigGetS>))]
[JsonSerializable(typeof(MossState))]
[JsonSerializable(typeof(ExtensionInfo))]
[JsonSerializable(typeof(ConfigSet))]
[JsonSerializable(typeof(ConfigGetS))]
[JsonSerializable(typeof(ConfigGetD))]
[JsonSerializable(typeof(PygameExtraRect))]
[JsonSerializable(typeof(PygameExtraRectEdgeRounding))]
[JsonSerializable(typeof(Rect))]
[JsonSerializable(typeof(Screen))]
[JsonSerializable(typeof(Dictionary<string, object>))]
internal partial class JsonContext : JsonSerializerContext
{
}