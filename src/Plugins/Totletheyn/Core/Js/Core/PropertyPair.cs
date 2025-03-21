using System;
using Totletheyn.Core.Js.BaseLibrary;

namespace Totletheyn.Core.Js.Core;

#if !NETCORE
[Serializable]
#endif
public sealed class PropertyPair
{
    internal Function getter;
    internal Function setter;

    internal PropertyPair()
    {
    }

    public PropertyPair(Function getter, Function setter)
    {
        this.getter = getter;
        this.setter = setter;
    }

    public Function Getter => getter;
    public Function Setter => setter;

    public override string ToString()
    {
        var tempStr = "[";
        if (getter != null)
            tempStr += "Getter";
        if (setter != null)
            tempStr += tempStr.Length != 1 ? "/Setter" : "Setter";
        if (tempStr.Length == 1)
            return "[Invalid Property]";
        tempStr += "]";
        return tempStr;
    }
}