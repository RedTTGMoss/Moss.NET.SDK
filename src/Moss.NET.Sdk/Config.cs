using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public static class Config
{
    public static void Set(string key, object value)
    {
        Functions.SetConfig(Utils.Serialize(new ConfigSet(key, value), JsonContext.Default.ConfigSet));
    }

    public static T Get<T>(string key)
    {
        var ptr = Functions.GetConfig(Utils.Serialize(new ConfigGet(key), JsonContext.Default.ConfigGet));
        return (T)Utils.Deserialize(ptr, JsonContext.Default.ConfigGet).value;
    }
}