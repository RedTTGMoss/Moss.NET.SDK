using Moss.NET.Sdk.API;

namespace Moss.NET.Sdk.Storage;

public abstract class StorageItem
{
    public Metadata Metadata { get; protected set; }
}