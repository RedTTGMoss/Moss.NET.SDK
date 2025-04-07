using System.Collections.Generic;
using Hocon;
using Moss.NET.Sdk.Scheduler;

namespace Totletheyn.Core.Eventing;

public class EventTrigger
{
    public string Trigger { get; set; }
    public HoconObject Config { get; set; }
    public List<IEventAction> Actions { get; set; }

    public static Activator<IEventAction> Activator { get; set; } = new();

    public static EventTrigger Create(string trigger, HoconObject config)
    {
        var eventTrigger = new EventTrigger
        {
            Trigger = trigger,
            Config = config
        };

        var actions = new List<IEventAction>();
        foreach (var action in config["do"].GetObject())
        {
            var actionTypeName = action.Key;
            var actionType = Activator.Create(actionTypeName);
            actions.Add(actionType);
        }

        eventTrigger.Actions = actions;

        return eventTrigger;
    }

}