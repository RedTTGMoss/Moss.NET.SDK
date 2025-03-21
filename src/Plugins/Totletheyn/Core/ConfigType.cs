using Moss.NET.Sdk;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core;

public class ConfigType : CustomType
{
    protected internal override JSValue GetProperty(JSValue key, bool forWrite, PropertyScope propertyScope)
    {
        var value = Config.Get<object>(key.ToString());

        return Wrap(value);
    }
}