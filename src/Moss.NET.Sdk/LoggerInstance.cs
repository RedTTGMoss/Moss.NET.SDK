using Extism;

namespace Moss.NET.Sdk;

public class LoggerInstance(string typeName)
{
    public void Log(LogLevel level, string message)
    {
        Pdk.Log(level, $"[{typeName}] {message}");
    }
    
    public void Debug(string message) => Log(LogLevel.Debug, message);
    public void Info(string message) => Log(LogLevel.Info, message);
    public void Warning(string message) => Log(LogLevel.Warn, message);
    public void Error(string message) => Log(LogLevel.Error, message);
}