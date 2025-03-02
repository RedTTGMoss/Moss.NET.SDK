﻿using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.UI;

public class ContextMenu(string key, List<ContextButton> buttons)
{
    [JsonPropertyName("key")]
    public string Key { get; init; } = key;

    [JsonPropertyName("buttons")]
    public List<ContextButton> Buttons { get; init; } = buttons;

    [DllImport(Functions.DLL, EntryPoint = "moss_gui_open_context_menu")]
    private static extern void OpenContextMenu(ulong keyPtr, ulong x, ulong y);

    public static ContextMenuBuilder Create(string name) => new(name);

    internal static readonly Dictionary<string, ContextMenu> Cache = new();

    public static ContextMenu Get(string name) => Cache[name];

    public void Open(ulong x, ulong y) => OpenContextMenu(this.GetPointer(), x, y);
}