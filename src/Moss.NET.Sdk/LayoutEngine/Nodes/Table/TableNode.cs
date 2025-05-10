using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes.Table;

public class TableNode : YogaNode
{
    public readonly TableRowNode HeaderRow;

    public TableNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        FlexDirection = YogaFlexDirection.Column;
        HeaderRow = AddRow("header");
    }

    public YogaValue CellPadding
    {
        get => HeaderRow.Padding;
        set
        {
            foreach (var rows in this) rows.Padding = value;
        }
    }

    public TableRowNode AddRow(string? name = null)
    {
        TableRowNode row = new(Config, ParentLayout)
        {
            Name = name
        };

        Add(row);
        return row;
    }


    public YogaNode AddColumn(string? header = null, YogaAlign? align = null, bool isBold = true)
    {
        var cell = HeaderRow.AddCell();

        if (header != null)
        {
            var headerTextNode = ParentLayout.CreateTextNode(header);
            headerTextNode.AutoSize = true;
            headerTextNode.FontSize = 13;
            headerTextNode.AlignSelf = align ?? YogaAlign.Center;
            headerTextNode.IsBold = isBold;

            cell.Add(headerTextNode);
        }

        return cell;
    }

    public override void ReCalculate(PdfPageBuilder page)
    {
        base.ReCalculate(page);

        if (HeaderRow.Display == YogaDisplay.None)
            foreach (var cell in HeaderRow)
                cell.GetChild(0).Display = YogaDisplay.None;
    }

    public void AlternateColor(Color first, Color second)
    {
        var startIndex = 1;
        if (HeaderRow.Display == YogaDisplay.None) startIndex = 0;

        for (var i = startIndex; i < Count; i++)
            if (this[i] is TableRowNode tableRow)
                tableRow.Background = i % 2 == 0 ? first : second;
    }

    public TextNode GetColumn(int index)
    {
        if (HeaderRow[index] is TableCellNode cell) return (cell.GetChild(0) as TextNode)!;

        throw new ArgumentOutOfRangeException(nameof(index), "Column index out of range");
    }
}