﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Scheduler;
using Moss.NET.Sdk.Storage;
using Moss.NET.Sdk.UI;

namespace SdkTesterPlugin;

public class SampleExtension : MossExtension
{
    private static readonly LoggerInstance _logger = Log.GetLogger<SampleExtension>();
    private Document duplicate;

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_register")]
    public static ulong Register()
    {
        TaskScheduler.Activator.Register<TestJob>("test");

        Init<SampleExtension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override void Register(MossState state)
    {
        MossConfig.Set("theme", "dark");
        Defaults.SetDefaultValue("OUTLINE_COLOR", Color.Blue);
        Theme.Apply(new DarkTheme());

        Assets.Add("extension/Assets/swap.svg");
        Assets.Add("extension/Assets/test.pdf");
        Assets.Add("extension/Assets/test.epub");

        Moss.NET.Sdk.Moss.RegisterExtensionButton(
            new ContextButton("Sample Button", "notebook", "notebook", "dispatch_entry",
                ""));

        var cm = ContextMenu.Create("test_cm")
            .AddButton("test", "swap", "notebook", "dispatch_entry")
            .Build();
    }

    public override void ExtensionLoop(MossState state)
    {
        Defaults.GetDefaultColor("BACKGROUND");
        Defaults.GetDefaultTextColor("TEXT_COLOR");
        Defaults.GetDefaultValue<string>("LOG_FILE");
        MossConfig.Get<string>("theme");
        Moss.NET.Sdk.Moss.GetState();

        var cm = ContextMenu.Get("test_cm");

        GUI.InvertIcon("swap", "swap_inverted");

        RunOnce.Execute("test", void () =>
        {
            cm.Open(10, 10);

            ScreenManager.Open<SampleScreen>(new Dictionary<string, object>
            {
                { "hello", true }
            });

            _logger.Info($"Current Date: {DateTime.Now}");

            var quickSheets = Document.Get("0ba3df9c-8ca0-4347-8d7c-07471101baad");
            _logger.Info($"Metadata: {quickSheets.Metadata.VisibleName} with {quickSheets.Metadata.Hash}");

            duplicate = quickSheets.Duplicate();

            var doc = new Document("test notebook");
            InternalFunctions.NewContentNotebook(1);

            //var customCollection = new Collection("Test collection");

            var pdf = new PdfNotebook("test pdf", "extension/Assets/test.pdf");
            InternalFunctions.NewContentPdf();

            _logger.Info($"Pdf Document: {pdf.Metadata.VisibleName}");

            var epub = new EpubNotebook("test ebook", "extension/Assets/test.epub");
            InternalFunctions.NewContentEpub();

            pdf.Upload();

            InternalFunctions.ExportDocument("0ba3df9c-8ca0-4347-8d7c-07471101baad");

            quickSheets.EnsureDownload();
            quickSheets.LoadFilesFromCache();
            quickSheets.UnloadFiles();

            quickSheets.RandomizeUUIDs();

            InternalFunctions.GetLoaderProgress();
            var ri = InternalFunctions.GetRootInfo();
            _logger.Info("Hash: " + ri.Hash);
            _logger.Info("Generation: " + ri.Generation);

            var kid = InternalFunctions.NewFileSyncProgress();
            _logger.Info("FileSyncProgress: " + kid);

            /* doesn't work yet
             InternalFunctions.NewDocumentSyncProgress(new Accessor
             {
                 Type = AccessorType.FileSyncProgress,
                 Id = kid,
                 Uuid = quickSheets.Metadata.Accessor.Uuid,
             });*/

            duplicate.EnsureDownload();
            duplicate.Metadata.Get<string>("visible_name");

            duplicate.Metadata.Set("visible_name", "Duplicated QuickSheet");
            duplicate.Metadata.Set("parent", "4dba1a54-93b8-4992-886f-08c0b17f93da");

            var k = duplicate.Duplicate();
            duplicate.Upload();
            k.Delete(() => _logger.Info("Deleted"));

            var docs = Enumerable.Repeat(0, 2)
                .Select(_ => k.Duplicate())
                .ToArray();

            InternalFunctions.SpreadEvent(new Accessor { Type = AccessorType.SyncStage });

            StorageFunctions.UploadManyDocuments(docs);
            StorageFunctions.DeleteManyDocuments(docs);
        });
    }

    public override void Unregister()
    {
        ScreenManager.Close();
        InternalFunctions.ExportStatisticalData();
    }
}