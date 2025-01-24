using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public static class Config
{
    public static void Set(string key, object value)
    {
        FFI.Functions.SetConfig(Utils.Serialize(new ConfigSet(key, value), FFI.JsonContext.Default.ConfigSet));
    }

    public static T Get<T>(string key)
    {
        var ptr = FFI.Functions.GetConfig(Utils.Serialize(new ConfigGet(key), FFI.JsonContext.Default.ConfigGet));
        return (T)Utils.Deserialize(ptr, FFI.JsonContext.Default.ConfigGet).value;
    }
}