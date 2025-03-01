using Moss.NET.Sdk.UI;

namespace Moss.NET.Sdk;

//ToDo: implement custom screen management to avoid using names
public abstract class Screen
{
    public abstract string Name { get; }

    public WidgetCollection Widgets { get; } = new();

    protected abstract void OnLoop();
    public virtual void PreLoop(){}
    public virtual void PostLoop(){}

    public void Close()
    {
        ScreenManager.CloseMossScreen();
    }

    public void Loop()
    {
        Widgets.Render();

        OnLoop();
    }

    protected void AddWidget(Widget widget)
    {
        Widgets.Add(widget);
    }
}