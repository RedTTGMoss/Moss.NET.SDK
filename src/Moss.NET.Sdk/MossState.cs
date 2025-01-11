using System.Collections.Generic;

namespace Moss.NET.Sdk;

public record MossState(int width, int height, string current_screen, List<string> opened_context_menus, List<string> icons);