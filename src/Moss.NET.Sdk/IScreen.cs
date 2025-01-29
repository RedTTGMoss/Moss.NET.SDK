namespace Moss.NET.Sdk;

public interface IScreen
{
    static abstract string Name { get; }

    static abstract void Loop();
}