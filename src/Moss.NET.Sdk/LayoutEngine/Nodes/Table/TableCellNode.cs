namespace Moss.NET.Sdk.LayoutEngine.Nodes.Table;

public class TableCellNode : YogaNode
{
    public TableCellNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        Padding = 2;
        FlexDirection = YogaFlexDirection.Column;
    }
}