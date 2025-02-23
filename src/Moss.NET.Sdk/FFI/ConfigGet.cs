using System.Text.Json;

namespace Moss.NET.Sdk.FFI;

internal record ConfigGetD(JsonElement value);
internal record ConfigGetS(object value);