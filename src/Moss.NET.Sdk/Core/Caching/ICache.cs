using PolyType;

namespace Moss.NET.Sdk.Core.Caching;

public interface ICache
{
    public void Set<T>(string key, T value)
        where T : IShapeable<T>;

    public T? Get<T>(string key)
        where T : IShapeable<T>;
}