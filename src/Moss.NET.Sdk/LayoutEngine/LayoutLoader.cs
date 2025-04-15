using System;
using Moss.NET.Sdk.LayoutEngine.Nodes;

namespace Moss.NET.Sdk.LayoutEngine;

using System.Xml.Linq;

public static class LayoutLoader
{
    public static Layout LoadLayoutFromXml(string xmlContent)
    {
        var xml = XDocument.Parse(xmlContent);

        var device = Enum.Parse<Device>(xml.Root!.Attribute("device")!.Value, true);
        var isLandscape = xml.Root.Attribute("isLandscape")?.Value == "true";
        var layout = Layout.Create(device, isLandscape);

        layout.GetRoot().SetAttributes(xml.Root);

        foreach (var child in xml.Root.Elements())
        {
            var node = ParseNode(layout, child);
            if (node != null)
            {
                layout.Add(node);
            }
        }

        return layout;
    }

    private static YogaNode? ParseNode(Layout layout, XElement element)
    {
        YogaNode? node;
        switch (element.Name.LocalName)
        {
            case "text":
                node = layout.CreateTextNode(element.Attribute("text")?.Value ?? string.Empty,
                    element.Attribute("name")?.Value);
                if (!string.IsNullOrEmpty(element.Value))
                {
                    ((TextNode)node).Text = element.Value;
                }
                break;
            case "hr":
                node = layout.CreateHorizontalLine();
                break;
            case "img":
                node = layout.CreateImageNode(element.Attribute("src")!.Value, element.Attribute("name")?.Value);
                break;
            default:
                if (element.FirstNode is XText t)
                {
                    node = layout.CreateTextNode(t.Value,
                        element.Attribute("name")?.Value);
                    break;
                }

                node = layout.CreateNode(element.Attribute("name")?.Value);
                break;
        }

        if (node == null) return null;

        node.SetAttributes(element);

        foreach (var child in element.Elements())
        {
            var childNode = ParseNode(layout, child);
            if (childNode != null)
            {
                node.Add(childNode);
            }
        }

        return node;
    }
}