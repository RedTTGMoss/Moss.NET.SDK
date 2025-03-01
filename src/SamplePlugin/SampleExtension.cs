using System.Collections.Generic;
using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.NEW;

namespace SamplePlugin;

public class SampleExtension : MossExtension
{
    [UnmanagedCallersOnly(EntryPoint = "moss_extension_register")]
    public static ulong Register()
    {
        Init<SampleExtension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override ExtensionInfo Register(MossState state)
    {
        Pdk.Log(LogLevel.Info, "registered sample extension");

        Config.Set("theme", "dark");
        Defaults.SetDefaultValue("OUTLINE_COLOR", Color.Blue);
        Theme.Apply(new DarkTheme());
        Storage.DuplicateDocument("0ba3df9c-8ca0-4347-8d7c-07471101baad");

        return new ExtensionInfo([]);
    }

    public override void ExtensionLoop(MossState state)
    {
      /*  ScreenManager.OpenScreen<SampleScreen>(new Dictionary<string, object>()
        {
            { "hello", true }
        });*/

        Defaults.GetDefaultColor("BACKGROUND");
        Defaults.GetDefaultTextColor("TEXT_COLOR");
        Defaults.GetDefaultValue<string>("LOG_FILE");
        Config.Get<string>("theme");
        Moss.NET.Sdk.Moss.GetState();

        var md = Storage.GetDocumentMetadata("0ba3df9c-8ca0-4347-8d7c-07471101baad");
        Pdk.Log(LogLevel.Info, $"Metadata: {md.VisibleName} with {md.Hash}");

        InternalFunctions.ExportStatisticalData();
    }

    public override void Unregister()
    {
        ScreenManager.CloseScreen();
        Pdk.Log(LogLevel.Info, "unregistered sample extension");
    }
}