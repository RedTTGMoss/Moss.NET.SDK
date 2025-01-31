namespace Moss.NET.Sdk;

//ToDo: implement custom screen management to avoid using names
public interface IScreen
{
    static abstract string Name { get; }

    static abstract void Loop();
}