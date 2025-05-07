using PolyType;

namespace Moss.NET.Sdk.Core.Caching;

public class DefaultPersistendCache : ICache
{
    private const string Path = "extension/.cache";

    public DefaultPersistendCache()
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
    }

    public void Set<T>(string key, T value)
        where T : IShapeable<T>
    {
        var encoded = PolyType.Examples.CborSerializer.CborSerializer.Encode(value);

        var itemPath = System.IO.Path.Combine(Path, key);
        if (!File.Exists(itemPath))
        {
            File.Create(itemPath).Close();
        }

        File.WriteAllBytes(itemPath, encoded);
    }

    public T? Get<T>(string key)
        where T : IShapeable<T>
    {
        try
        {
            var raw = File.ReadAllBytes(System.IO.Path.Combine(Path, key));
            var cacheItem = PolyType.Examples.CborSerializer.CborSerializer.Decode<T>(raw);

            return cacheItem;
        }
        catch
        {
            return default;
        }
    }
}