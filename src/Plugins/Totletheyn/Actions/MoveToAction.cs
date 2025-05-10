using Moss.NET.Sdk;
using Moss.NET.Sdk.Storage;
using Totletheyn.Core.Eventing;

namespace Totletheyn.Actions;

public class MoveToAction : IEventAction
{
    public string Name => "move to";

    public void Execute(Metadata md, object collection)
    {
        var collectionID = MossConfig.Get<string>((string)collection);
    }
}