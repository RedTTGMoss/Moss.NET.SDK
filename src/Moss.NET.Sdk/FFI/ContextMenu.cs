using System.Collections.Generic;

namespace Moss.NET.Sdk.FFI;

public record ContextMenu(string key, List<ContextButton> buttons);