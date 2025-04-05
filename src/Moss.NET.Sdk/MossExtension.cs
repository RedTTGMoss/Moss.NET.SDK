﻿using System;
using System.Diagnostics.Metrics;
using System.Text.Json;
using Extism;
using Hocon;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.Core.Instrumentation;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.Scheduler;

namespace Moss.NET.Sdk;

public class MossExtension
{
    private static MossExtension? _instance;

    public Meter? Meter;
    private IMeterListener? _meterListener;

    internal static MossExtension? Instance
    {
        get
        {
            if (_instance is null) throw new InvalidOperationException("Extension is not initialized.");

            return _instance;
        }
        set => _instance = value;
    }

    public virtual void Register(MossState state)
    {

    }

    public virtual void Unregister()
    {
    }

    public virtual void ExtensionLoop(MossState state)
    {
    }

    public static void PreInitExtension()
    {
        var configPath = "extension/plugin.conf";

        if (System.IO.File.Exists(configPath)) {
            var configSource = System.IO.File.ReadAllText(configPath);
            Config = HoconParser.Parse(configSource);
        }

        TaskScheduler.Init();
    }

    public static HoconRoot Config { get; set; } = HoconParser.Parse("");

    public static void Init<T>() where T : MossExtension, new()
    {
        if (_instance is not null) throw new InvalidOperationException("Assembly can only have one extension instance.");

        PreInitExtension();
        _instance = Activator.CreateInstance<T>();

        var input = Pdk.GetInputJson(JsonContext.Default.MossState);

        if (Config!.GetBoolean("instrumentation.enabled", true))
        {
            _instance.Meter = new Meter("Moss.NET.Sdk");
            _instance._meterListener = new FileMeterListener("extension/instrumentation.txt");
            _instance._meterListener.Init();
        }

        Extensions.Measure("extension.register", () =>
        {
            _instance!.Register(input!);
        });


        var extensionInfo = new ExtensionInfo
        {
            Files = Assets.Expose()
        };

        var output = JsonSerializer.Serialize(extensionInfo, JsonContext.Default.ExtensionInfo);
#if DEBUG
        Pdk.Log(LogLevel.Info, "output: " + output);
#endif

        Pdk.SetOutput(output);
    }
}