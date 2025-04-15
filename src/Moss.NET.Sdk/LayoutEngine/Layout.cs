using System;
using System.Collections.Generic;
using System.IO;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine;

public class Layout
{
    private YogaNode root;
    private YogaConfig config;
    private readonly PdfPageBuilder? _page;
    public static PdfDocumentBuilder Builder;
    static Dictionary<string, PdfDocumentBuilder.AddedFont> _fonts = new();

    protected Layout(YogaConfig config, PdfPageBuilder? page, PdfRectangle pageSize = default)
    {
        this.config = config;
        _page = page;

        if (page != null)
        {
            pageSize = page.PageSize;
        }

        root = new YogaNode(config)
        {
            Width = pageSize.Width,
            Height = pageSize.Height,
            Margin = 10,
            Padding = 10,
            Name = "root",
            StyleDirection = YogaDirection.LeftToRight,
            ParentLayout = this
        };

        //todo: add standard font from moss
    }

    public YogaNode GetRoot() => root;

    public static Layout Create(PdfPageBuilder page)
    {
        return new Layout(new(), page);
    }

    public static Layout Create(Device device, bool isLandscape = true)
    {
        var dimension = device.GetDimension(isLandscape);
        return new Layout(new(), Builder.AddPage(dimension.width, dimension.height));
    }

    public static Layout CreateTemplate()
    {
        return new Layout(new(), null);
    }

    public YogaNode CreateNode(string? name = null)
    {
        return new YogaNode(config)
        {
            Name = name,
            ParentLayout = this
        };
    }

    public TextNode CreateTextNode(string text, string? name = null)
    {
        return new TextNode(config)
        {
            Name = name,
            Text = text,
            ParentLayout = this
        };
    }

    public void Add(YogaNode node)
    {
        root.Add(node);
    }

    public void Apply()
    {
        ReCalculate(root, _page);
        root.CalculateLayout();

        root.Draw(_page, 0, 0);
        DrawNode(root, _page);
    }

    private static void DrawNode(YogaNode root, PdfPageBuilder page, double offsetX = 0, double offsetY = 0)
    {
        foreach (var child in root.Children)
        {
            var absoluteX = offsetX + child.LayoutX;
            var absoluteY = offsetY + child.LayoutY;

            child.Draw(page, absoluteX, absoluteY);

            DrawNode(child, page, absoluteX, absoluteY);
        }
    }

    private static void ReCalculate(YogaNode root, PdfPageBuilder page)
    {
        foreach (var child in root.Children)
        {
            child.ReCalculate(page);

            ReCalculate(child, page);
        }
    }

    public HorizontalLineNode CreateHorizontalLine(string? name = null)
    {
        return new HorizontalLineNode(config){
            Name = name,
            ParentLayout = this
        };
    }

    /// <summary>
    /// Finds a node in the layout tree by a query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <example>content left article</example>
    public T? FindNode<T>(string query)
        where T : YogaNode
    {
        return root.FindNode<T>(query);
    }

    public static void AddFont(string name, string path)
    {
       var font = Builder.AddTrueTypeFont(File.ReadAllBytes(path));
       _fonts[name] = font;
    }

    public void EnableDebugLines()
    {
        var random = new Random();
        SetDebugLines(root, random);
    }

    private void SetDebugLines(YogaNode node, Random random)
    {
        node.BorderColor = new Color(
            (byte)random.Next(0, 256),
            (byte)random.Next(0, 256),
            (byte)random.Next(0, 256)
        );

        foreach (var child in node.Children)
        {
            SetDebugLines(child, random);
        }
    }

    public PdfDocumentBuilder.AddedFont GetFont(string fontFamily)
    {
        if (_fonts.TryGetValue(fontFamily, out var font))
        {
            return font;
        }

        //todo: use standard font instead of exception
        throw new ArgumentException($"Font '{fontFamily}' not found.");
    }

    public ImageNode CreateImageNode(string url, string? name = null)
    {
        return new ImageNode(config)
        {
            Name = name,
            ParentLayout = this,
            Src = new Uri(url)
        };
    }
}