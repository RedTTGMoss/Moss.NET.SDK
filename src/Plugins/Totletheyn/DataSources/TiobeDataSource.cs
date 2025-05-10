using System;
using System.Linq;
using System.Xml.Linq;
using HtmlAgilityPack;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using Moss.NET.Sdk.LayoutEngine.Nodes.Table;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources;

public class TiobeDataSource : IDataSource
{
    public string Name => "tiobe";

    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (node is not ContainerNode container)
            throw new ArgumentException("node is not a ContainerNode");

        var maxItems = Math.Min(int.Parse(element.Attribute("maxItems")?.Value ?? "10"), 20);

        container.Content.FlexDirection = YogaFlexDirection.Column;
        container.Content.JustifyContent = YogaJustify.FlexStart;
        container.Content.BorderColor = Colors.Black;
        container.Content.BorderStyle = BorderStyle.Dashed;
        container.Content.Padding = 5;

        var url = "https://www.tiobe.com";
        var httpClient = new RestTemplate();
        var html = httpClient.GetString($"{url}/tiobe-index");

        var doc = new HtmlDocument
        {
            OptionFixNestedTags = true,
            OptionAutoCloseOnEnd = true,
            OptionCheckSyntax = false
        };
        doc.LoadHtml(html);

        var table = doc.DocumentNode.SelectSingleNode("//table[@id='top20']");
        if (table == null)
            throw new Exception("TIOBE-Table not found");

        var rows = table.SelectNodes(".//tbody//tr");
        if (rows == null)
            throw new Exception("No rows found");

        container.Title = "TIOBE Index Top 10";
        container.TitleNode.FontSize = 15;
        container.TitleNode.IsBold = true;
        container.Copyright = "tiobe.com";

        var tableNode = node.ParentLayout.CreateTableNode("table");

        tableNode.AddColumn("Rank", YogaAlign.FlexStart);
        tableNode.AddColumn("Change", YogaAlign.FlexStart);
        tableNode.AddColumn("Language", YogaAlign.FlexStart);
        tableNode.AddColumn("Rating", YogaAlign.FlexStart);
        tableNode.AddColumn("Change", YogaAlign.FlexStart);

        tableNode.HeaderRow.Background = Colors.Gray;

        AddRows(node, rows, maxItems, tableNode, url);

        tableNode.AlternateColor(Colors.White, Colors.LightGray);

        tableNode.CellPadding = 5;
        container.Content.Add(tableNode);
    }

    private static void AddRows(YogaNode node, HtmlNodeCollection rows, int maxItems, TableNode tableNode, string url)
    {
        foreach (var row in rows.Take(maxItems))
        {
            var cells = row.SelectNodes("td")!;
            var rowNode = tableNode.AddRow();
            var cell = rowNode.AddCell();

            var rankNode = node.ParentLayout.CreateTextNode(cells[0].InnerText.Trim());
            rankNode.FontSize = 10;
            rankNode.FontFamily = "NoticiaText";
            rankNode.AutoSize = true;
            cell.Add(rankNode);

            cell = rowNode.AddCell();
            var change = cells[2].FirstChild?.GetAttributeValue("src", "");
            var changeNode = node.ParentLayout.CreateImageNode(url + change);
            changeNode.Width = 10;
            changeNode.Height = 10;
            cell.Add(changeNode);

            cell = rowNode.AddCell();
            var languageIcon = cells[3].FirstChild.GetAttributeValue("src", "");
            var languageIconNode = node.ParentLayout.CreateImageNode(url + languageIcon);
            languageIconNode.Width = 10;
            languageIconNode.Height = 10;

            var language = cells[4].InnerText.Trim();
            var languageTextNode = node.ParentLayout.CreateTextNode(language);
            languageTextNode.FontSize = 10;
            languageTextNode.FontFamily = "NoticiaText";
            languageTextNode.AutoSize = true;
            languageTextNode.AlignItems = YogaAlign.Center;

            var languageNode = node.ParentLayout.CreateNode("language");
            languageNode.FlexDirection = YogaFlexDirection.Row;
            languageNode.Gap = 10;
            languageNode.AlignItems = YogaAlign.Center;

            languageNode.Add(languageIconNode);
            languageNode.Add(languageTextNode);
            cell.Add(languageNode);

            cell = rowNode.AddCell();
            var rating = cells[5].InnerText.Trim().TrimStart('+', '-').TrimEnd('%');
            var ratingTextNode = node.ParentLayout.CreateTextNode(rating);
            ratingTextNode.FontSize = 10;
            ratingTextNode.FontFamily = "NoticiaText";
            ratingTextNode.AutoSize = true;
            ratingTextNode.TextFormat = "{0} %";
            cell.Add(ratingTextNode);

            cell = rowNode.AddCell();
            var changeValue = cells[6].InnerText.Trim().TrimStart('+', '-').TrimEnd('%');
            var changeValueTextNode = node.ParentLayout.CreateTextNode(double.Parse(changeValue));
            changeValueTextNode.FontSize = 10;
            changeValueTextNode.FontFamily = "NoticiaText";
            changeValueTextNode.AutoSize = true;
            changeValueTextNode.TextFormat = "{0:+0.00;-0.00} %";

            cell.Add(changeValueTextNode);
        }
    }
}