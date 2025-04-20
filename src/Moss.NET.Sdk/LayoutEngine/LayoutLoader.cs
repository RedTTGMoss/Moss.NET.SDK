using Moss.NET.Sdk.LayoutEngine.Nodes;
using System.Xml.Linq;
using Moss.NET.Sdk.LayoutEngine.DataSources;

namespace Moss.NET.Sdk.LayoutEngine;

public static class LayoutLoader
{
    private static readonly Dictionary<string, IDataSource> DataSources = new()
    {
        ["meta"] = new Meta()
    };

    public static void AddDataSource<T>()
        where T:IDataSource, new()
    {
        var component = new T();
        DataSources[component.Name] = component;
    }

    public static Layout LoadLayoutFromXml(string xmlContent)
    {
        var xml = XDocument.Parse(xmlContent);

        var device = Enum.Parse<Device>(xml.Root!.Attribute("device")!.Value, true);
        var isLandscape = xml.Root.Attribute("isLandscape")?.Value == "true";
        var layout = Layout.Create(device, isLandscape);

        if (xml.Root.Attribute("name") != null)
        {
            layout.Name = xml.Root.Attribute("name")!.Value;
        }

        layout.GetRoot().SetAttributes(xml.Root);

        foreach (var child in xml.Root.Elements())
        {
            var node = ParseNode(layout, child);
            if (node != null)
            {
                layout.Add(node);
            }
        }

        if (xml.Root.Attribute("debug")?.Value == "true")
        {
            layout.EnableDebugLines();
        }

        return layout;
    }

    public static Layout Load(string file)
    {
        return LoadLayoutFromXml(Layout.PathResolver.ReadText(file));
    }

    public static YogaNode LoadFragment(string file)
    {
        return LoadFragmentFromXml(Layout.PathResolver.ReadText(file));
    }

    public static YogaNode LoadFragmentFromXml(string xmlContent)
    {
        var xml = XDocument.Parse(xmlContent);

        var layout = Layout.CreateTemplate();
        foreach (var child in xml.Root!.Elements())
        {
            var node = ParseNode(layout, child);
            if (node != null)
            {
                layout.Add(node);
            }
        }

        if (xml.Root.Attribute("debug")?.Value == "true")
        {
            layout.EnableDebugLines();
        }
        layout.GetRoot().Name = xml.Root.Name.LocalName;

        return layout.GetRoot();
    }

    private static YogaNode? ParseNode(Layout layout, XElement element)
    {
        if (!CreateNode(layout, element, out var node) && node is null) return null;

        node!.SetAttributes(element);

        ParseChildren(layout, element, node);

        ApplyDataSource(layout, element, node);

        return node;
    }

    private static void ApplyDataSource(Layout layout, XElement element, YogaNode node)
    {
        if (element.Attribute("datasource") is not null
            && DataSources.TryGetValue(element.Attribute("datasource")!.Value, out var dataSource))
        {
            dataSource.ApplyData(node, layout.Page!, element);
        }
    }

    private static bool CreateNode(Layout layout, XElement element, out YogaNode? node)
    {
        switch (element.Name.LocalName)
        {
            case "setter":
                node = null;
                return false;
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
                node = layout.CreateImageNode(element.Attribute("src")?.Value!, element.Attribute("name")?.Value);
                break;
            case "container":
                node = layout.CreateContainerNode(element.Attribute("title")?.Value ?? string.Empty,
                    element.Attribute("copyright")?.Value ?? string.Empty,
                    element.Attribute("name")?.Value);
                break;
            case "fragment":
                node = LoadFragment(element.Attribute("src")!.Value);
                ApplyFragmentSetter(element, node);
                break;
            default:
                if (element.FirstNode is XText t)
                {
                    node = layout.CreateTextNode(t.Value,
                        element.Attribute("name")?.Value);
                    break;
                }

                var name = element.Attribute("name")?.Value;
                if (name is null)
                {
                    name = element.Name.LocalName;
                }

                node = layout.CreateNode(name);

                break;
        }

        return true;
    }

    private static void ParseChildren(Layout layout, XElement element, YogaNode node)
    {
        foreach (var child in element.Elements())
        {
            var childNode = ParseNode(layout, child);
            if (childNode != null)
            {
                if (node is ContainerNode container)
                {
                    container.Content.Add(childNode);
                }
                else
                {
                    node.Add(childNode);
                }
            }
        }
    }

    private static void ApplyFragmentSetter(XElement element, YogaNode node)
    {
        foreach (var setter in element.Elements())
        {
            if (setter.Name.LocalName != "setter") continue;

            var query = setter.Attribute("query")!.Value;
            var property = setter.Attribute("property")!.Value;
            var value = setter.Value;

            node.FindNode(query)?.SetAttribute(property, value);
        }
    }
}