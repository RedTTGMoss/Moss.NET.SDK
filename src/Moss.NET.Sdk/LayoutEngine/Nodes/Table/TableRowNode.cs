using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes.Table;

public class TableRowNode : YogaNode
{
    public TableRowNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        FlexDirection = YogaFlexDirection.Row;
        AlignItems = YogaAlign.Center;
    }

    public TableCellNode AddCell()
    {
        TableCellNode cell = new(Config, ParentLayout);
        Add(cell);

        return cell;
    }

    public override void ReCalculate(PdfPageBuilder page)
    {
        base.ReCalculate(page);

        var table = (TableNode)Parent;

        for (var index = 0; index < Count; index++)
        {
            var cell = this[index];

            cell.Width = YogaValue.Percent(1.0 / table.HeaderRow.Children.Count * 100);
        }
    }
}