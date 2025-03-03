using System.Collections.Generic;
using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.UI;

public class ContextMenuBuilder(string name)
{
    [DllImport(Functions.DLL, EntryPoint = "moss_gui_register_context_menu")]
    private static extern void RegisterContextMenu(ulong ptr);

    private readonly List<ContextButton> _buttons = [];

    public ContextMenuBuilder AddButton(string text, string icon, string contextIcon, string action)
    {
        var button = new ContextButton(text, icon, contextIcon, action, name);
        _buttons.Add(button);

        return this;
    }

    public ContextMenu Build()
    {
        var cm = new ContextMenu(name, _buttons);
        RegisterContextMenu(cm.GetPointer());
        ContextMenu.Cache[name] = cm;

        return cm;
    }
}