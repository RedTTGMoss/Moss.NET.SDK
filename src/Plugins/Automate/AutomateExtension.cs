using System;
using System.Runtime.InteropServices;
using Automate.Core;
using Extism;
using Moss.NET.Sdk;
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

         var test = """
                    let counter = 0;
                    on("import")
                       .do(md => move(md, Config.inbox));
                       
                    every("second")
                        .where(_ => counter % 2 == 0)
                        .do(() => log(counter++));
                    """;

         context.Eval("log('hello from js')");
         context.Eval(test);

         TaskScheduler.LoadTaskInformation();
    }

    private static void InitContext(Context context)
    {
        context.DefineVariable("Config").Assign(new ConfigType());
        context.DefineVariable("move").Assign(JSValue.Marshal(new Action<object, string>((md, name) =>
        {
            Logger.Info("move " + name);
        })));

        context.DefineVariable("log").Assign(JSValue.Marshal(new Action<object>(x => Pdk.Log(LogLevel.Info, x.ToString()))));
        context.DefineVariable("on").Assign(JSValue.Marshal(new Func<string, ScheduleBuilder>(ScheduleBuilder.on)));
        context.DefineVariable("every").Assign(JSValue.Marshal(new Func<string, ScheduleBuilder>(ScheduleBuilder.every)));
    }

    public override void ExtensionLoop(MossState state)
    {
        TaskScheduler.CheckTasks();
    }
}