using Moss.NET.Sdk.Storage;

namespace Totletheyn.Core.Eventing;

public interface IEventAction
{
    static abstract string Name { get; }
    void Execute(Metadata md, object param);
}