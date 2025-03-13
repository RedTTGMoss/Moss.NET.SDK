using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Formats.Core;
using Moss.NET.Sdk.Storage;

namespace EpubTest;

public class SampleExtension : MossExtension
{
    private static LoggerInstance _logger = Log.GetLogger<SampleExtension>();

    [ModuleInitializer]
    public static void ModInit()
    {
        Init<SampleExtension>();
    }

    public static void Main()
    {
    }

    public override void ExtensionLoop(MossState state)
    {
        RunOnce.Execute("epub", async void () =>
        {
            var output = new Base64();
            var writer = new EpubWriter();

            var template = File.ReadAllText("extension/Assets/template.html");

            _logger.Info("Template: " + template);

            writer.SetTitle("Generated");
            writer.AddAuthor("furesoft");
            writer.AddChapter("Chapter 1", template);
            writer.AddChapter("Chapter 2", template);
            writer.AddChapter("Chapter 3", template);

            writer.Write(output);

            var epub = new EpubNotebook("test ebook", output);

            await epub.UploadAsync();
        });
    }
}