using PolyType;
using PolyType.Examples.CborSerializer;

namespace Moss.NET.Sdk.Core.Caching;

public class DefaultPersistendCache : ICache
{
    private const string Path = "extension/.cache";
    private static readonly LoggerInstance Logger = Log.GetLogger<DefaultPersistendCache>();

    public DefaultPersistendCache()
    {
        if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
    }

    public void Set<T>(string key, T value)
        where T : IShapeable<T>
    {
        var encoded = CborSerializer.Encode(value);

        var itemPath = System.IO.Path.Combine(Path, key);
        if (!File.Exists(itemPath)) File.Create(itemPath).Close();

        File.WriteAllBytes(itemPath, encoded);
    }

    public T? Get<T>(string key)
        where T : IShapeable<T>
    {
        if (!HasKey(key)) return default;

        try
        {
            var raw = File.ReadAllBytes(System.IO.Path.Combine(Path, key));
            var cacheItem = CborSerializer.Decode<T>(raw);

            return cacheItem;
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
            return default;
        }
    }

    public void Remove(string key)
    {
        var path = System.IO.Path.Combine(Path, key);

        if (File.Exists(path)) File.Delete(path);
    }

    public bool HasKey(string key)
    {
        var path = System.IO.Path.Combine(Path, key);

        if (!File.Exists(path))
        {
            Logger.Error($"Cache item {key} not found");
            return false;
        }

        return File.Exists(path);
    }

    public void Clear()
    {
        var files = Directory.GetFiles(Path);
        foreach (var file in files)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }
    }
}