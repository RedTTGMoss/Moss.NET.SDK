namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class ContainerNode : YogaNode
{
    private readonly TextNode copyrightNode;
    private readonly TextNode titleNode;

    public ContainerNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        Content = ParentLayout!.CreateNode("content");
        FlexDirection = YogaFlexDirection.Column;

        titleNode = ParentLayout.CreateTextNode("", "title");
        titleNode.AlignSelf = YogaAlign.Center;
        titleNode.PositionType = YogaPositionType.Relative;
        titleNode.Margin = 5;
        titleNode.AutoSize = true;

        copyrightNode = ParentLayout.CreateTextNode("© ", "copyright");
        copyrightNode.AlignSelf = YogaAlign.FlexEnd;
        copyrightNode.PositionType = YogaPositionType.Relative;
        copyrightNode.FontFamily = "NoticiaText";
        copyrightNode.FontSize = 6;
        copyrightNode.AutoSize = true;
        copyrightNode.MarginTop = 5;

        Content.FlexGrow = 1;
        Content.Margin= 5;

        Margin = 5;

        if (Title is not null)
        {
            Add(titleNode);
        }

        Add(Content);

        if (Copyright is not null)
        {
            Add(copyrightNode);
        }
    }

    public required string Title
    {
        get => titleNode.Text;
        set => titleNode.Text = value;
    }

    public required string Copyright
    {
        get => copyrightNode.Text;
        set => copyrightNode.Text = "© " + value;
    }

    public YogaNode Content { get; }

    internal override void SetAttribute(string name, string value)
    {
        switch (name)
        {
            case "title":
                Title = value;
                break;
            case "copyright":
                Copyright = value;
                break;
        }
    }
}