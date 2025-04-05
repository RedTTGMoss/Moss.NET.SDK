using System.Collections.Generic;
using System.Text.Json.Serialization;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk;

public class MossState
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("current_screen")]
    public string CurrentScreen { get; set; }

    [JsonPropertyName("opened_context_menus")]
    public List<string> OpenedContextMenus { get; set; }

    [JsonPropertyName("icons")]
    public string[] Icons { get; set; }
}