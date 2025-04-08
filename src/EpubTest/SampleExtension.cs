using System.IO;
using System.Runtime.InteropServices;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Formats.Core;
using Moss.NET.Sdk.Storage;

namespace EpubTest;

public class SampleExtension : MossExtension
{
    private static readonly LoggerInstance _logger = Log.GetLogger<SampleExtension>();

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_register")]
    public static ulong Register()
    {
        Init<SampleExtension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override void ExtensionLoop(MossState state)
    {
        RunOnce.Execute("epub", void () =>
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

            epub.Upload();
        });
    }
}