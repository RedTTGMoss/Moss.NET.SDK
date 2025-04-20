namespace Moss.NET.Sdk;

public static class Log
{
    private static readonly Dictionary<string, LoggerInstance> Cache = new();

    public static LoggerInstance GetLogger<T>()
    {
        return GetLogger(typeof(T).Name);
    }

    public static LoggerInstance GetLogger(string name)
    {
        if (!Cache.TryGetValue(name, out var loggerInstance))
        {
            loggerInstance = new LoggerInstance(name);
            Cache[name] = loggerInstance;
        }

        return loggerInstance;
    }
}