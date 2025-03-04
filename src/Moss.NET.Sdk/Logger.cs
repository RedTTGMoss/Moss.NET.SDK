using System.Collections.Generic;

namespace Moss.NET.Sdk;

public static class Log
{
    private static readonly Dictionary<string, LoggerInstance> Cache = new();

    public static LoggerInstance GetLogger<T>()
    {
        var typeName = typeof(T).Name;
        if (!Cache.TryGetValue(typeName, out var loggerInstance))
        {
            loggerInstance = new LoggerInstance(typeName);
            Cache[typeName] = loggerInstance;
        }

        return loggerInstance;
    }
}