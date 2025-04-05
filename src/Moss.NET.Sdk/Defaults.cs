using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;
using Moss.NET.Sdk.UI;

namespace Moss.NET.Sdk;

public static class Defaults
{
    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_set_color")]
    private static extern void SetDefaultColor(ulong keyPtr, ulong colorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_get_text_color")]
    private static extern ulong GetDefaultTextColor(ulong keyPtr); // -> TextColors

    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_set_text_color")]
    private static extern void SetDefaultTextColor(ulong keyPtr, ulong textColorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_get")]
    private static extern ulong GetDefaultValue(ulong keyPtr); //-> ConfigGet

    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_get_color")]
    private static extern ulong MossGetDefaultColor(ulong keyPtr); // -> Color

    [DllImport(Functions.DLL, EntryPoint = "_moss_defaults_set")]
    private static extern void SetDefaultValue(ulong configSetPtr);

    public static void SetDefaultColor(string key, Color color)
    {
        SetDefaultColor(key.GetPointer(), color.GetPointer());
    }

    public static void SetDefaultTextColor(string key, Color foreground, Color background)
    {
        var textColor = new TextColor(foreground, background);

        SetDefaultTextColor(key.GetPointer(), textColor.GetPointer());
    }

    public static void SetDefaultValue(string key, object value)
    {
        var valuePtr = new ConfigSet(key, value).GetPointer();

        SetDefaultValue(valuePtr);
    }

    public static T GetDefaultValue<T>(string key)
    {
        var valuePtr = GetDefaultValue(key.GetPointer());

        return valuePtr.Get<ConfigGetD>().value
            .Deserialize((JsonTypeInfo<T>)JsonContext.Default.GetTypeInfo(typeof(T))!)!;
    }

    public static Color GetDefaultColor(string key)
    {
        var valuePtr = MossGetDefaultColor(key.GetPointer());

        return valuePtr.Get<Color>();
    }

    public static TextColor GetDefaultTextColor(string key)
    {
        var textColorPtr = GetDefaultTextColor(key.GetPointer());

        return textColorPtr.Get<TextColor>();
    }
}