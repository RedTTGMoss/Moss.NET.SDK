using System;
using System.Text.Json;
using System.Xml.Linq;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Writer;

namespace Totletheyn.DataSources.Crypto;

public class CryptoDataSource : IDataSource
{
    public string Name => "crypto";
    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (node is not ContainerNode container)
            throw new ArgumentException("node is not a ContainerNode");

        container.Title = "Crypto Prices";
        container.Copyright = "coingecko.com";

        container.Content.FlexDirection = YogaFlexDirection.Row;

        var template = new RestTemplate();
        template.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; AcmeInc/1.0)");

        var coins = (string[])["bitcoin", "ethereum", "worldcoin-wld"];
        var currency = "eur";
        var url = $"https://api.coingecko.com/api/v3/coins/markets?ids={string.Join(',', coins)}&vs_currency={currency}&include_24hr_change=true";
        var response = template.GetString(url);
        var cyptos = JsonSerializer.Deserialize<CryptoPrice[]>(response);

        foreach (var crypto in cyptos)
        {
            var tile = node.ParentLayout.CreateNode(crypto.Name);
            tile.FlexDirection = YogaFlexDirection.Column;
            tile.Padding = 10;
            tile.Margin = 10;
            tile.Width = 180;
            tile.Background = Colors.LightGray;
            tile.BorderColor = Colors.Gray;
            tile.BorderStyle = BorderStyle.Solid;
            tile.Gap = 5;

            // Header: Bild, Name, Symbol
            var header = node.ParentLayout.CreateNode("header");
            header.FlexDirection = YogaFlexDirection.Row;
            header.AlignItems = YogaAlign.Center;
            header.Gap = 10;

            var img = node.ParentLayout.CreateImageNode(crypto.Image);
            img.Width = 32;
            img.Height = 32;
            header.Add(img);

            var nameNode = node.ParentLayout.CreateTextNode(crypto.Name);
            nameNode.FontSize = 12;
            nameNode.TextFormat = $"{{0}} ({crypto.Symbol})";

            header.Add(nameNode);
            tile.Add(header);

            var table = node.ParentLayout.CreateTableNode();
            table.AddColumn("Price", YogaAlign.FlexStart);
            table.AddColumn("24h Change", YogaAlign.FlexStart);

            var row = table.AddRow();
            var priceCell = row.AddCell();
            var priceText = node.ParentLayout.CreateTextNode($"{crypto.CurrentPrice:0.00} €");
            priceText.FontSize = 11;
            priceText.AutoSize = true;
            priceCell.Add(priceText);

            var changeCell = row.AddCell();
            var changeText = node.ParentLayout.CreateTextNode($"{crypto.PriceChangePercentage24h:+0.00;-0.00} %");
            changeText.FontSize = 11;
            changeText.AutoSize = true;
            changeText.Color = crypto.PriceChangePercentage24h >= 0 ? Colors.Green : Colors.Red;
            changeText.AlignSelf = YogaAlign.FlexEnd;
            changeCell.Add(changeText);

            table.AlternateColor(Colors.White, Colors.WhiteSmoke);
            table.CellPadding = 4;

            tile.Add(table);

            container.Content.Add(tile);
        }
    }
}