using System.Collections.Generic;

namespace Moss.NET.Sdk;

public record ContextMenu(string key, List<ContextButton> buttons);