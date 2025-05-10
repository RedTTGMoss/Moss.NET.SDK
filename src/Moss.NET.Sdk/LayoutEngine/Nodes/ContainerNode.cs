namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class ContainerNode : YogaNode
{
    public readonly TextNode CopyrightNode;
    public readonly TextNode TitleNode;

    public ContainerNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        Content = ParentLayout.CreateNode("content");
        FlexDirection = YogaFlexDirection.Column;

        TitleNode = ParentLayout.CreateTextNode("", "title");
        TitleNode.AlignSelf = YogaAlign.Center;
        TitleNode.PositionType = YogaPositionType.Relative;
        TitleNode.Margin = 5;
        TitleNode.AutoSize = true;

        CopyrightNode = ParentLayout.CreateTextNode("", "copyright");
        CopyrightNode.AlignSelf = YogaAlign.FlexEnd;
        CopyrightNode.PositionType = YogaPositionType.Relative;
        CopyrightNode.FontFamily = "NoticiaText";
        CopyrightNode.FontSize = 6;
        CopyrightNode.AutoSize = true;
        CopyrightNode.MarginTop = 5;
        CopyrightNode.TextFormat = "© {0}";

        Content.Margin = 2;

        Margin = 5;

        if (Title is not null) Add(TitleNode);

        Add(Content);

        if (Copyright is not null) Add(CopyrightNode);
    }

    public required string? Title
    {
        get => TitleNode.Text?.ToString();
        set => TitleNode.Text = value;
    }

    public required string? Copyright
    {
        get => CopyrightNode.Text?.ToString();
        set
        {
            if (string.IsNullOrEmpty(value)) return;

            CopyrightNode.Text = value;
        }
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