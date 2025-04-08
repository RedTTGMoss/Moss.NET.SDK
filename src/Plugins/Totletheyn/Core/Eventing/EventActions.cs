using System.Collections.Generic;
using Hocon;
using Totletheyn.Actions;

namespace Totletheyn.Core.Eventing;

public class EventActions
{
    public static List<EventTrigger> Triggers { get; } = [];

    public static void Init(HoconRoot config)
    {
        var eventConfig = config.GetObject("events", null);

        if (eventConfig is null) return;

        RegisterAction<MoveToAction>();

        foreach (var trigger in eventConfig)
        {
            var eventTrigger = EventTrigger.Create(trigger.Key, trigger.Value.GetObject());
            Triggers.Add(eventTrigger);
        }
    }

    static void RegisterAction<T>()
        where T : class, IEventAction, new()
    {
        EventTrigger.Activator.Register<T>(new T().Name);
    }
}