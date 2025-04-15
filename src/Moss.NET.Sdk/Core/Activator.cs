using System;
using System.Collections.Generic;

namespace Moss.NET.Sdk.Scheduler;

public class Activator<TOut>
{
    private readonly Dictionary<string, Type> _types = new();

    public void Register<T>(string name) where T : class
    {
        _types[name] = typeof(T);
    }

    public TOut Create(string name)
    {
        if (!_types.TryGetValue(name, out var type)) throw new Exception($"Type '{name}' not found");

        return (TOut)Activator.CreateInstance(type)! ?? throw new InvalidOperationException($"Job '{name}' not found");
    }
}