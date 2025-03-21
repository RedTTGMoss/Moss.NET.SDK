using System.Collections.Generic;
using Extism;
using Scriban;
using Scriban.Runtime;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Lib;

public static class Renderer
{
    public static string RenderObject(JSObject sobj, string content)
    {
        Pdk.Log(LogLevel.Error, "Before render Content: " + content);

        var context = new TemplateContext();
        context.PushGlobal(BuildScriptObject(sobj));
        context.EnableRelaxedMemberAccess = true;

        var template = Template.Parse(content);

        return template.Render(context);
    }

    private static ScriptObject BuildScriptObject(JSValue obj)
    {
        var sobj = new ScriptObject();

        foreach (var kv in obj)
        {
            var renamedKey = StandardMemberRenamer.Rename(kv.Key);

            if (kv.Value is JSObject value)
            {
                sobj.Add(renamedKey, BuildScriptObject(value));
            }
            else if (kv.Value is Array arr)
            {
                var items = new List<object>();

                foreach (var arrItem in arr)
                {
                    items.Add(BuildScriptObject(arrItem.Value));
                }

                sobj.Add(renamedKey, items.ToArray());
            }
            else
            {
                sobj.Add(renamedKey, kv.Value.Value);
            }
        }

        return sobj;
    }
}