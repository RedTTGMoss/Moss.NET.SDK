using System;

namespace Totletheyn.Core.Js.Core.Interop;

[AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
public sealed class JavaScriptNameAttribute : Attribute
{
    public JavaScriptNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}