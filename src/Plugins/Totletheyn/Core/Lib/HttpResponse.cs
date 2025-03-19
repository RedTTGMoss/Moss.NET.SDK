using Extism;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;

namespace Totletheyn.Core.Lib;

public class HttpResponse(MemoryBlock memory, ushort status) : Extism.HttpResponse(memory, status)
{
    public ulong status { get; } = status;

    public string text => Body.ReadString();
    public Uint8Array raw => new(new ArrayBuffer(Body.ReadBytes()));
    public JSValue json => JSON.parse(text);
}