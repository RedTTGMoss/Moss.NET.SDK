﻿using System;
using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;
using Moss.NET.Sdk.FFI;

namespace SamplePlugin;

public class SampleExtension : MossExtension
{
    [ModuleInitializer]
    public static void ModInit()
    {
        Init<SampleExtension>();
    }

    public static void Main()
    {
         
    }

    public override ExtensionInfo Register(MossState state)
    {
        Pdk.Log(LogLevel.Info, "Hello world form sample plugin");

        return new ExtensionInfo([]);
    }

    public override void Unregister()
    {
        
    }
}