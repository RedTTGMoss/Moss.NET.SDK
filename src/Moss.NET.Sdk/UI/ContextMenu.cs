using System.Collections.Generic;

namespace Moss.NET.Sdk.UI;

public record ContextMenu(string key, List<ContextButton> buttons);