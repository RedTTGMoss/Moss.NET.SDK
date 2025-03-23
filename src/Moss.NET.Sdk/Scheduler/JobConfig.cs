using System.Linq;

namespace Moss.NET.Sdk.Scheduler;

using System.Dynamic;
using Hocon;

public class JobConfig(HoconObject hoconObject) : DynamicObject
{
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        if (hoconObject.TryGetValue(binder.Name, out var hoconValue))
        {
            result = ConvertHoconValue(hoconValue.Value);
            return true;
        }

        result = null;
        return true;
    }

    private static object ConvertHoconValue(HoconValue hoconValue)
    {
        return hoconValue.Type switch
        {
            HoconType.Object => new JobConfig(hoconValue.GetObject()),
            HoconType.Array => hoconValue.GetArray().Select(ConvertHoconValue).ToArray(),
            HoconType.String => hoconValue.GetString(),
            HoconType.Boolean => hoconValue.GetBoolean(),
            HoconType.Number => hoconValue.GetDouble(),
            _ => hoconValue.GetString()
        };
    }
}