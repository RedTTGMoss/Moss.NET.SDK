using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Automate.Core;

[JsonSerializable(typeof(List<ScheduledTask>))]
public partial class JsonContext : JsonSerializerContext
{
    
}