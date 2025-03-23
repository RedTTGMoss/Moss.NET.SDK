using System;
using System.Collections.Generic;

namespace Moss.NET.Sdk.Scheduler;

public class JobActivator {
    private readonly Dictionary<string, Type> _types = new();
    
    public void Register<T>(string name) where T : class
    {
        _types[name] = typeof(T);
    }
    
    public Job Create(string name)
    {
        if (!_types.TryGetValue(name, out var type))
        {
            throw new Exception($"Type '{name}' not found");
        }

        return (Job)Activator.CreateInstance(type)! ?? throw new InvalidOperationException($"Job '{name}' not found");
    }
}