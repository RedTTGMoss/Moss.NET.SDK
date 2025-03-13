using Moss.NET.Sdk;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Automate.Core;

public class ConfigType : CustomType
{
    private static readonly LoggerInstance _logger = Log.GetLogger<ConfigType>();
    protected override JSValue GetProperty(JSValue key, bool forWrite, PropertyScope propertyScope)
    {
        var value = Config.Get<object>(key.ToString());
        _logger.Info("get " + key + " = " + value);

        return JSValue.Wrap(value);
    }
}