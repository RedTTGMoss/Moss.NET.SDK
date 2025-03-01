using System.Collections.Generic;

namespace Moss.NET.Sdk.UI;

public class WidgetCollection : List<Widget>
{
    public void Render()
    {
        foreach (var widget in this)
            if (widget.IsVisible)
                widget.Render();
    }
}