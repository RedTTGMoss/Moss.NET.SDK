using System;
using System.IO;
using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Formats.Core;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core;
using Totletheyn.Core.CustomSyntax;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Lib;
using HttpRequest = Totletheyn.Core.Lib.HttpRequest;

namespace Totletheyn;

public class AutomateExtension : MossExtension
{
    private static readonly LoggerInstance Logger = Log.GetLogger<AutomateExtension>();

    [UnmanagedCallersOnly(EntryPoint = "moss_extension_register")]
    public static ulong Register()
    {
        Init<AutomateExtension>();

        return 0;
    }

    public static void Main()
    {
    }

    public override void Register(MossState state)
    {
        Parser.DefineCustomCodeFragment(typeof(EveryStatement));
        Parser.DefineCustomCodeFragment(typeof(OnStatement));

        var context = new Context();
        InitContext(context);

        EvalScript(context);
    }

    private void EvalScript(Context context)
    {
        var source = File.ReadAllText("extension/automation.js");
        context.Eval(source);

        Logger.Info("Automation script loaded");
    }

    private static void InitContext(Context context)
    {
        context.DefineVariable("Config").Assign(new ConfigType());

        context.DefineFunction("log", new Action<object>(x => Pdk.Log(LogLevel.Info, x.ToString())));
        context.DefineFunction("render", Renderer.RenderObject);
        context.DefineFunction("_on", ScheduleBuilder.on);
        context.DefineFunction("_every", ScheduleBuilder.every);

        context.DefineConstructor(typeof(EpubWriter));
        context.DefineConstructor(typeof(Guid));
        context.DefineConstructor(typeof(Base64));

        context.DefineFunction("newEpub", new Func<string, Base64, EpubNotebook>((name, data) => new EpubNotebook(name, data)));

        context.DefineConstructor(typeof(EpubNotebook));
        context.DefineConstructor(typeof(HttpRequest));
        context.DefineConstructor(typeof(Metadata));
    }
}