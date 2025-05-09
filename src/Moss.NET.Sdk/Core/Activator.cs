namespace Moss.NET.Sdk.Scheduler;

public class Activator<TOut>
{
    private readonly Dictionary<string, Type> _types = new();
    private readonly Dictionary<Type, string> _names = new();

    public void Register<T>(string name) where T : class
    {
        _types[name] = typeof(T);
        _names[typeof(T)] = name;
    }

    public TOut Create(string name)
    {
        if (!_types.TryGetValue(name, out var type)) throw new Exception($"Type '{name}' not found");

        return (TOut)Activator.CreateInstance(type)! ?? throw new InvalidOperationException($"Job '{name}' not found");
    }

    public string GetName(Type type)
    {
        if (!_names.TryGetValue(type, out var name)) throw new Exception($"Type '{type.Name}' not found");

        return name;
    }

    public string GetName(TOut instance)
    {
        return GetName(instance!.GetType());
    }
}