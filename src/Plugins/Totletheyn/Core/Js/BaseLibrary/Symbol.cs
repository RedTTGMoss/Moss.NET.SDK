using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
[DisallowNewKeyword]
public sealed class Symbol : JSValue
{
    private static readonly Dictionary<string, Symbol> symbolsCache = new();

    public static readonly Symbol iterator = new("iterator");
    public static readonly Symbol toStringTag = new("toStringTag");

    public Symbol()
        : this("")
    {
    }

    public Symbol(string description)
    {
        Description = description;
        _oValue = this;
        _valueType = JSValueType.Symbol;
        if (!symbolsCache.ContainsKey(description))
            symbolsCache[description] = this;
    }

    [JavaScriptName("description")] public string Description { [Hidden] get; private set; }

    public static Symbol @for(string description)
    {
        Symbol result = null;
        symbolsCache.TryGetValue(description, out result);
        return result ?? new Symbol(description);
    }

    public static string keyFor(Symbol symbol)
    {
        if (symbol == null)
            ExceptionHelper.ThrowTypeError("Invalid argument");

        return symbol.Description;
    }

    public override JSValue toString(Arguments args)
    {
        return ToString();
    }

    [Hidden]
    public override string ToString()
    {
        return "Symbol(" + Description + ")";
    }
}