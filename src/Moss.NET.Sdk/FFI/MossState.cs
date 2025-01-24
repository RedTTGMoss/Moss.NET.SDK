using System.Collections.Generic;

namespace Moss.NET.Sdk.FFI;

public record MossState(
    int width,
    int height,
    string current_screen,
    List<string> opened_context_menus,
    List<string> icons);