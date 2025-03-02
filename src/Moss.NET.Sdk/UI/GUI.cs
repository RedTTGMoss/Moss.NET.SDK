using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.UI;

public class GUI
{
    [DllImport(Functions.DLL, EntryPoint = "moss_gui_invert_icon")]
    private static extern void InvertIcon(ulong keyPtr, ulong resultKeyPtr);

}