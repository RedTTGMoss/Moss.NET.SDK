using System;

namespace Moss.NET.Sdk.Storage;

public class Collection : StorageItem<Collection>
{
    public Collection(string name)
    {
        throw new NotImplementedException();
    }

    public Collection()
    {
    }

    public StandaloneId ID { get; }
}