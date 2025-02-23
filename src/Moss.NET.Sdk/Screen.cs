namespace Moss.NET.Sdk;

//ToDo: implement custom screen management to avoid using names
public abstract class Screen
{
    public abstract string Name { get; }

    public abstract void Loop();
    public virtual void PreLoop(){}
    public virtual void PostLoop(){}

    public void Close()
    {
        ScreenManager.CloseMossScreen();
    }
}