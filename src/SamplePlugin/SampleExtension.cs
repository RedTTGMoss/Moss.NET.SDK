using System.Collections.Generic;
using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.UI;

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

        Assets.Add("Assets/swap.svg");

        var md = Storage.GetDocumentMetadata("0ba3df9c-8ca0-4347-8d7c-07471101baad");
        Pdk.Log(LogLevel.Info, $"Metadata: {md.VisibleName} with {md.Hash}");

        var nid = Storage.DuplicateDocument("0ba3df9c-8ca0-4347-8d7c-07471101baad");

        //var testUuid = Storage.NewNotebook("test notebook");
        //Storage.NewPdf("test pdf", Array.Empty<byte>());
      //  Storage.NewEpub("test ebook", Array.Empty<byte>());

        Storage.UnloadFiles("0ba3df9c-8ca0-4347-8d7c-07471101baad");
        Storage.RandomizeUUIDs("0ba3df9c-8ca0-4347-8d7c-07471101baad");

        Moss.NET.Sdk.Moss.RegisterExtensionButton(
            new ContextButton("Sample Button", "notebook", "notebook", "no_action",
                "no_contextmenu"));

        ContextMenu.Create("test_cm")
            .AddButton("test", "swap", "notebook", "no_action")
            .Build();

        return new ExtensionInfo();
    }

    public override void ExtensionLoop(MossState state)
    {
        ScreenManager.Open<SampleScreen>(new Dictionary<string, object>()
        {
            { "hello", true }
        });

        Defaults.GetDefaultColor("BACKGROUND");
        Defaults.GetDefaultTextColor("TEXT_COLOR");
        Defaults.GetDefaultValue<string>("LOG_FILE");
        Config.Get<string>("theme");
        Moss.NET.Sdk.Moss.GetState();

        var cm = ContextMenu.Get("test_cm");

        cm.Open(10, 10);

        GUI.InvertIcon("swap", "swap_inverted");

        InternalFunctions.ExportStatisticalData();
    }

    public override void Unregister()
    {
        ScreenManager.Close();
        Pdk.Log(LogLevel.Info, "unregistered sample extension");
    }
}