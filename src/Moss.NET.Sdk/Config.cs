using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public static class Config
{
    public static void Set(string key, object value)
    {
        FFI.FFI.SetConfig(Utils.Serialize(new ConfigSet(key, value), FFI.JsonContext.Default.ConfigSet));
    }

    public static T Get<T>(string key)
    {
        var ptr = FFI.FFI.GetConfig(Utils.Serialize(new ConfigGet(key), FFI.JsonContext.Default.ConfigGet));
        return (T)Utils.Deserialize(ptr, FFI.JsonContext.Default.ConfigGet).value;
    }

    public static void SetDefaultColor(ColorKey key, Color color)
    {
        FFI.FFI.SetDefaultColor(Pdk.Allocate(key.ToString()).Offset, color.r, color.g, color.b, color.a);
    }

    public static void SetDefaultTextColor(TextColorKey key, Color color)
    {
        FFI.FFI.SetDefaultTextColor(Pdk.Allocate(key.ToString()).Offset, color.r, color.g, color.b, color.a);
    }
}