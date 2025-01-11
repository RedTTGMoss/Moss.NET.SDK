using System;
using Moss.NET.Sdk;

namespace SamplePlugin;

public class SampleExtension : MossExtension
{
    public static void Main()
    {
        Load<SampleExtension>();
    }

    public override ExtensionInfo Register(MossState state)
    {
        return new ExtensionInfo([]);
    }

    public override void Unregister()
    {
        Console.WriteLine("unregistered sample extension");
    }
}