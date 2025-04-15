using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.UI;

//ToDo: implement custom screen management to avoid using names
public abstract class Screen
{
    public abstract string Name { get; }

    public WidgetCollection Widgets { get; } = [];

    [DllImport(Functions.DLL, EntryPoint = "moss_pe_get_screen_value")]
    private static extern ulong GetScreenValue(ulong keyPtr); //-> ConfigGet

    [DllImport(Functions.DLL, EntryPoint = "_moss_pe_set_screen_value")]
    private static extern void SetScreenValue(ulong configSetPtr);

    protected abstract void OnLoop();

    public virtual void PreLoop()
    {
    }

    public virtual void PostLoop()
    {
    }

    public void Close()
    {
        ScreenManager.CloseMossScreen();
    }

    public void Loop()
    {
        Widgets.Render();

        OnLoop();
    }

    protected void AddWidget(Widget widget)
    {
        Widgets.Add(widget);
    }

    public void SetValue(string key, object value)
    {
        SetScreenValue(Utils.Serialize(new ConfigSet(key, value), JsonContext.Default.ConfigSet));
    }

    public T? GetValue<T>(string key)
    {
        var valuePtr = GetScreenValue(Pdk.Allocate(key).Offset);

        var value = Utils.Deserialize(valuePtr, JsonContext.Default.ConfigGetD)?.value;

        if (!value.HasValue) return default;

        return value.Value.GetValue<T>();
    }
}