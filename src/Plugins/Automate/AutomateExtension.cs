﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using Automate.Core;
using Extism;
using Moss.NET.Sdk;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Formats.Core;
using Moss.NET.Sdk.Storage;
using NiL.JS.Core;

namespace Automate;

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
        context.DefineVariable("move").Assign(JSValue.Marshal(new Action<object, string>((md, name) =>
        {
            Logger.Info("move " + name);
        })));

        context.DefineVariable("log")
            .Assign(JSValue.Marshal(new Action<object>(x => Pdk.Log(LogLevel.Info, x.ToString()))));
        context.DefineVariable("on").Assign(JSValue.Marshal(new Func<string, ScheduleBuilder>(ScheduleBuilder.on)));
        context.DefineVariable("every")
            .Assign(JSValue.Marshal(new Func<string, ScheduleBuilder>(ScheduleBuilder.every)));

        context.DefineConstructor(typeof(EpubWriter));
        context.DefineConstructor(typeof(EpubNotebook));
        context.DefineConstructor(typeof(Base64));

        context.DefineConstructor(typeof(Automate.Core.Lib.HttpRequest));
    }
}