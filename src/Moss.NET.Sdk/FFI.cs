using System.Runtime.InteropServices;

namespace Moss.NET.Sdk;

public static class FFI
{
    [DllImport("extism", EntryPoint = "moss_gui_register_menu")]
    public static extern int RegisterMenu();

    [DllImport("extism", EntryPoint = "moss_em_config_get")]
    public static extern int GetConfig(ulong ptr);
}