using Moss.NET.Sdk;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Automate.Core;

public class ConfigType : CustomType
{
    protected override JSValue GetProperty(JSValue key, bool forWrite, PropertyScope propertyScope)
    {
        var value = Config.Get<object>(key.ToString());

        return Wrap(value);
    }
}