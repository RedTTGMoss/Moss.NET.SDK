using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk;

public static class MossConfig
{
    [DllImport(Functions.DLL, EntryPoint = "moss_em_config_get")]
    private static extern ulong GetConfig(ulong ptr); //-> ConfigGet

    [DllImport(Functions.DLL, EntryPoint = "_moss_em_config_set")]
    private static extern void SetConfig(ulong ptr);

    public static void Set(string key, object value)
    {
        SetConfig(new ConfigSet(key, value).GetPointer());
    }

    public static T Get<T>(string key)
    {
        var ptr = GetConfig(key.GetPointer());
        return ptr.Get<ConfigGetD>().value.GetValue<T>();
    }
}