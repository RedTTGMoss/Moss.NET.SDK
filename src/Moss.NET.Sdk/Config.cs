using Extism;

namespace Moss.NET.Sdk;

public static class Config
{
    public static void Set(string key, object value)
    {
        FFI.SetConfig(Utils.Serialize(new ConfigSet(key, value), JsonContext.Default.ConfigSet));
    }

    public static T Get<T>(string key)
    {
        var ptr = FFI.GetConfig(Utils.Serialize(new ConfigGet(key), JsonContext.Default.ConfigGet));
        return (T)Utils.Deserialize(ptr, JsonContext.Default.ConfigGet).value;
    }

    public static void SetDefaultColor(ColorKey key, Color color)
    {
        FFI.SetDefaultColor(Pdk.Allocate(key.ToString()).Offset, color.r, color.g, color.b, color.a);
    }
}