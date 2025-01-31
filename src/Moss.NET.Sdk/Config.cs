using System.Runtime.InteropServices;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public static class Config
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_config_get")]
    private static extern ulong GetConfig(ulong ptr); //-> ConfigGet

    [DllImport(Functions.DLL, EntryPoint = "_moss_em_config_set")]
    private static extern void SetConfig(ulong ptr);

    public static void Set(string key, object value)
    {
        SetConfig(Utils.Serialize(new ConfigSet(key, value), JsonContext.Default.ConfigSet));
    }

    public static T Get<T>(string key)
    {
        var ptr = GetConfig(Utils.Serialize(new ConfigGet(key), JsonContext.Default.ConfigGet));
        return (T)Utils.Deserialize(ptr, JsonContext.Default.ConfigGet).value;
    }
}