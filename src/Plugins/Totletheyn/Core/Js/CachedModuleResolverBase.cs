using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js;

public abstract class CachedModuleResolverBase : IModuleResolver
{
    private readonly StringMap<Module> _modulesCache = new();

    bool IModuleResolver.TryGetModule(ModuleRequest moduleRequest, out Module result)
    {
        var cacheKey = GetCacheKey(moduleRequest);

        if (_modulesCache.TryGetValue(cacheKey, out result))
            return true;

        if (TryGetModule(moduleRequest, out result))
        {
            _modulesCache.Add(cacheKey, result);
            return true;
        }

        return false;
    }

    public abstract bool TryGetModule(ModuleRequest moduleRequest, out Module result);

    public virtual string GetCacheKey(ModuleRequest moduleRequest)
    {
        return moduleRequest.AbsolutePath;
    }

    public void RemoveFromCache(string key)
    {
        _modulesCache.Remove(key);
    }

    public void ClearCache()
    {
        _modulesCache.Clear();
    }
}