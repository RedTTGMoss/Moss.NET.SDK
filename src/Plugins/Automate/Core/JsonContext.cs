using System.Collections.Generic;
using System.Text.Json.Serialization;
using Automate.Core.Scheduler;

namespace Automate.Core;

[JsonSerializable(typeof(List<ScheduledTask>))]
public partial class JsonContext : JsonSerializerContext
{
}