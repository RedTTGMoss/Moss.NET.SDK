namespace Moss.NET.Sdk.UI;

public abstract class Widget
{
    public string? Name { get; set; }
    public ulong Id { get; init; }
    public bool IsVisible { get; set; } = true;

    protected abstract void OnRender();

    public void Render()
    {
        if (IsVisible) OnRender();
    }
}