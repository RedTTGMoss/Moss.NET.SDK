using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Automate.Core;
using Extism;
using Moss.NET.Sdk;
using NiL.JS.Core;

namespace Automate;

public class AutomateExtension : MossExtension
{
    private static readonly LoggerInstance _logger = Log.GetLogger<AutomateExtension>();

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

         var test = "on(\"import\").do(md => move(md, Config.inbox))";

         context.Eval("log('hello from js')");
         context.Eval(test);
    }

    private static void InitContext(Context context)
    {
        context.DefineConstant("import", "import");

        context.DefineVariable("Config").Assign(new ConfigType());
        context.DefineVariable("move").Assign(JSValue.Marshal(new Action<object, string>((md, name) =>
        {
            _logger.Info("move " + name);
        })));

        context.DefineVariable("log").Assign(JSValue.Marshal(new Action<string>(x => Pdk.Log(LogLevel.Info, x))));

        context.DefineVariable("on").Assign(JSValue.Marshal(new Func<string, JSValue>(ev =>
        {
            _logger.Info("on ");

            var obj = JSObject.CreateObject();
            obj.DefineProperty("where").Assign(JSValue.Marshal(new Func<ICallable, JSObject>(x =>
            {
                _logger.Info("where ");
                _logger.Info(x.ToString());

                return obj;
            })));
            obj.DefineProperty("do").Assign(JSValue.Marshal(new Func<ICallable, JSObject>(c =>
            {
                _logger.Info("do ");
                _logger.Info(c.ToString());
                c.Call(JSValue.Undefined, new Arguments());

                return obj;
            })));

            return obj;
        })));
    }

    public override void Unregister()
    {
    }
}