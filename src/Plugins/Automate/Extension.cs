using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk;
using NiL.JS.Core;

namespace Automate;

public class Extension : MossExtension
{
    [ModuleInitializer]
    public static void ModInit()
    {
        Init<Extension>();
    }

    public static void Main()
    {
    }

    public override void Register(MossState state)
    {
         var context = new Context();
         context.DefineVariable("log").Assign(JSValue.Marshal(new Action<string>(x => Pdk.Log(LogLevel.Info, x))));

         context.Eval("log('hello from js')");
    }

    public override void Unregister()
    {
    }
}