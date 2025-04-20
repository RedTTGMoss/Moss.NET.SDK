/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Globalization;
using System.Xml.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine;

public partial class YogaNode : IEnumerable<YogaNode>
{
    public const double DefaultFlexGrow = 0.0f;
    public const double DefaultFlexShrink = 0.0f;
    public const double WebDefaultFlexShrink = 1.0f;

    private static int _instanceCount = 0;

    private YogaPrint _print;
    private bool _hasNewLayout;
    private YogaNodeType _nodeType;
    private YogaMeasure _measure;
    private YogaBaseline _baseline;
    private bool _isReferenceBaseline;
    private YogaDirtied _dirtied;
    private YogaStyle _style;
    private YogaLayout _layout;
    private int _lineIndex;
    private YogaNode _owner;
    private List<YogaNode> _children;
    private YogaNode _nextChild;
    private YogaConfig _config;
    private bool _isDirty;
    private YogaArray<YogaValue> _resolvedDimensions; // [2]
    public Layout ParentLayout { get; set; }
    public Color? Background { get; set; }
    public BoxShadow? BoxShadow { get; set; }

    public string ID { get; set; }

    /// <summary>
    /// The gap between each children
    /// </summary>
    public YogaValue Gap { get; set; }

    public YogaNode()
    {
        _print = null;
        _hasNewLayout = true;
        _nodeType = YogaNodeType.Default;
        _measure = null;
        _baseline = null;
        _dirtied = null;
        _style = new YogaStyle();
        _layout = new YogaLayout();
        _lineIndex = 0;
        _owner = null;
        _children = new List<YogaNode>();
        _nextChild = null;
        _config = new YogaConfig();
        _isDirty = false;
        _resolvedDimensions = new YogaArray<YogaValue>(new YogaValue[] { YogaValue.Unset, YogaValue.Unset });

        Interlocked.Increment(ref _instanceCount);
    }

    public YogaNode(YogaNode node)
    {
        _print = node._print;
        _hasNewLayout = node._hasNewLayout;
        _nodeType = node._nodeType;
        _measure = node._measure;
        _baseline = node._baseline;
        _dirtied = node._dirtied;
        _style = node._style;
        _layout = node._layout;
        _lineIndex = node._lineIndex;
        _owner = node._owner;
        _children = new List<YogaNode>(node._children);
        _nextChild = node._nextChild;
        _config = node._config;
        _isDirty = node._isDirty;
        _resolvedDimensions = YogaArray.From(node._resolvedDimensions);

        Interlocked.Increment(ref _instanceCount);
    }

    public YogaNode(YogaNode node, YogaNode owner)
        : this(node)
    {
        _owner = owner;
    }

    public YogaNode(
        YogaPrint print,
        bool hasNewLayout,
        YogaNodeType nodeType,
        YogaMeasure measure,
        YogaBaseline baseline,
        YogaDirtied dirtied,
        YogaStyle style,
        YogaLayout layout,
        int lineIndex,
        YogaNode owner,
        List<YogaNode> children,
        YogaNode nextChild,
        YogaConfig config,
        bool isDirty,
        YogaValue[] resolvedDimensions)
    {
        _print = print;
        _hasNewLayout = hasNewLayout;
        _nodeType = nodeType;
        _measure = measure;
        _baseline = baseline;
        _dirtied = dirtied;
        _style = style;
        _layout = layout;
        _lineIndex = lineIndex;
        _owner = owner;
        _children = new List<YogaNode>(children);
        _nextChild = nextChild;
        _config = config;
        _isDirty = isDirty;
        _resolvedDimensions = YogaArray.From(resolvedDimensions);

        Interlocked.Increment(ref _instanceCount);
    }

    public YogaNode(YogaConfig config, Layout layout) : this()
    {
        _config = config ?? new YogaConfig();

        if (_config.UseWebDefaults)
        {
            Style.FlexDirection = YogaFlexDirection.Row;
            Style.AlignContent = YogaAlign.Stretch;
        }

        ParentLayout = layout;
    }

    ~YogaNode()
    {
        Interlocked.Decrement(ref _instanceCount);
    }

    // Getters

    public static int GetInstanceCount()
    {
        return _instanceCount;
    }

    public Color? BorderColor { get; set; }
    public string? Name { get; set; }

    public YogaPrint PrintFunction
    {
        get { return _print; }
        set { _print = value; }
    }

    public bool HasNewLayout
    {
        get { return _hasNewLayout; }
        set { _hasNewLayout = value; }
    }

    public YogaNodeType NodeType
    {
        get { return _nodeType; }
        set { _nodeType = value; }
    }

    public YogaMeasure Measure
    {
        get { return _measure; }
        set
        {
            if (_children.Count > 0)
                throw new InvalidOperationException("Cannot set measure function: Nodes with measure functions cannot have children.");

            if (value == null)
            {
                _measure = null;
                // TODO: t18095186 Move nodeType to opt-in function and mark appropriate
                // places in Litho
                _nodeType = YogaNodeType.Default;
            }
            else
            {
                _measure = value;
                // TODO: t18095186 Move nodeType to opt-in function and mark appropriate
                // places in Litho
                NodeType = YogaNodeType.Text;
            }

            _measure = value;
        }
    }

    public YogaBaseline Baseline
    {
        get { return _baseline; }
        set { _baseline = value; }
    }

    public YogaDirtied Dirtied
    {
        get { return _dirtied; }
        set { _dirtied = value; }
    }

    public YogaStyle Style
    {
        get { return _style; }
        set { _style.CopyFrom(value); }
    }

    public YogaLayout Layout
    {
        get { return _layout; }
        set { _layout.CopyFrom(value); }
    }

    public int LineIndex
    {
        get { return _lineIndex; }
        set { _lineIndex = value; }
    }

    public YogaNode Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public YogaNode Parent { get { return _owner; } }

    public List<YogaNode> Children
    {
        get { return _children; }
        set { _children = new List<YogaNode>(value ?? Enumerable.Empty<YogaNode>()); }
    }

    public YogaNode GetChild(int index) { return _children[index]; }

    public YogaNode NextChild
    {
        get { return _nextChild; }
        set { _nextChild = value; }
    }

    public YogaConfig Config
    {
        get { return _config; }
        set { _config = value; }
    }

    public bool IsDirty
    {
        get { return _isDirty; }
        set
        {
            if (value == _isDirty)
                return;

            _isDirty = value;
            if (value && _dirtied != null)
                _dirtied(this);
        }
    }

    public YogaArray<YogaValue> ResolvedDimensions { get { return _resolvedDimensions; } }

    public YogaValue GetResolvedDimension(YogaDimension index) { return _resolvedDimensions[index]; }

    // Methods related to positions, margin, padding and border
    public double? GetLeadingPosition(YogaFlexDirection axis, double? axisSize)
    {
        var leadingPosition = default(YogaValue);
        if (axis.IsRow())
        {
            leadingPosition = ComputedEdgeValue(_style.Position, YogaEdge.Start, YogaValue.Unset);
            if (leadingPosition.Unit != YogaUnit.Undefined)
                return leadingPosition.Resolve(axisSize);
        }

        leadingPosition = ComputedEdgeValue(_style.Position, Leading[axis], YogaValue.Unset);
        return leadingPosition.Unit == YogaUnit.Undefined ? 0.0f : leadingPosition.Resolve(axisSize);
    }

    public double? GetTrailingPosition(YogaFlexDirection axis, double? axisSize)
    {
        var trailingPosition = default(YogaValue);
        if (axis.IsRow())
        {
            trailingPosition = ComputedEdgeValue(_style.Position, YogaEdge.End, YogaValue.Unset);
            if (trailingPosition.Unit != YogaUnit.Undefined)
                return trailingPosition.Resolve(axisSize);
        }

        trailingPosition = ComputedEdgeValue(_style.Position, Trailing[axis], YogaValue.Unset);
        return trailingPosition.Unit == YogaUnit.Undefined ? 0.0f : trailingPosition.Resolve(axisSize);
    }

    public double? GetRelativePosition(YogaFlexDirection axis, double? axisSize)
    {
        return IsLeadingPositionDefined(axis) ? GetLeadingPosition(axis, axisSize) : -GetTrailingPosition(axis, axisSize);
    }

    public bool IsLeadingPositionDefined(YogaFlexDirection axis)
    {
        return (axis.IsRow() && ComputedEdgeValue(_style.Position, YogaEdge.Start, YogaValue.Unset).Unit != YogaUnit.Undefined)
               || ComputedEdgeValue(_style.Position, Leading[axis], YogaValue.Unset).Unit != YogaUnit.Undefined;
    }

    public bool IsTrailingPositionDefined(YogaFlexDirection axis)
    {
        return (axis.IsRow() && ComputedEdgeValue(_style.Position, YogaEdge.End, YogaValue.Unset).Unit != YogaUnit.Undefined)
               || ComputedEdgeValue(_style.Position, Trailing[axis], YogaValue.Unset).Unit != YogaUnit.Undefined;
    }

    public double? GetLeadingMargin(YogaFlexDirection axis, double? widthSize)
    {
        if (axis.IsRow() && _style.Margin[YogaEdge.Start].Unit != YogaUnit.Undefined)
            return ResolveValueMargin(_style.Margin[YogaEdge.Start], widthSize);

        return ResolveValueMargin(ComputedEdgeValue(_style.Margin, Leading[axis], YogaValue.Zero), widthSize);
    }

    public double? GetTrailingMargin(YogaFlexDirection axis, double? widthSize)
    {
        if (axis.IsRow() && _style.Margin[YogaEdge.End].Unit != YogaUnit.Undefined)
            return ResolveValueMargin(_style.Margin[YogaEdge.End], widthSize);

        return ResolveValueMargin(ComputedEdgeValue(_style.Margin, Trailing[axis], YogaValue.Zero), widthSize);
    }

    public double? GetMarginForAxis(YogaFlexDirection axis, double? widthSize)
    {
        return GetLeadingMargin(axis, widthSize) + GetTrailingMargin(axis, widthSize);
    }

    public YogaValue GetMarginLeadingValue(YogaFlexDirection axis)
    {
        if (axis.IsRow() && _style.Margin[YogaEdge.Start].Unit != YogaUnit.Undefined)
            return _style.Margin[YogaEdge.Start];

        return _style.Margin[Leading[axis]];
    }

    public YogaValue GetMarginTrailingValue(YogaFlexDirection axis)
    {
        if (axis.IsRow() && _style.Margin[YogaEdge.End].Unit != YogaUnit.Undefined)
            return _style.Margin[YogaEdge.End];

        return _style.Margin[Trailing[axis]];
    }

    public double GetLeadingBorder(YogaFlexDirection axis)
    {
        if (axis.IsRow()
            && _style.Border[YogaEdge.Start].Unit != YogaUnit.Undefined
            && _style.Border[YogaEdge.Start].Value != null
            && _style.Border[YogaEdge.Start].Value >= 0.0f)
        {
            return _style.Border[YogaEdge.Start].Value.Value;
        }

        var computedEdgeValue = ComputedEdgeValue(_style.Border, Leading[axis], YogaValue.Zero).Value;
        return YogaMath.Max(computedEdgeValue, 0.0F);
    }

    public double GetTrailingBorder(YogaFlexDirection flexDirection)
    {
        if (flexDirection.IsRow()
            && _style.Border[YogaEdge.End].Unit != YogaUnit.Undefined
            && _style.Border[YogaEdge.End].Value != null
            && _style.Border[YogaEdge.End].Value >= 0.0f)
        {
            return _style.Border[YogaEdge.End].Value.Value;
        }

        var computedEdgeValue = ComputedEdgeValue(_style.Border, Trailing[flexDirection], YogaValue.Zero).Value;
        return YogaMath.Max(computedEdgeValue, 0.0f);
    }

    public double GetLeadingPadding(YogaFlexDirection axis, double? widthSize)
    {
        var paddingEdgeStart = _style.Padding[YogaEdge.Start].Resolve(widthSize);
        if (axis.IsRow()
            && _style.Padding[YogaEdge.Start].Unit != YogaUnit.Undefined
            && paddingEdgeStart != null
            && paddingEdgeStart >= 0.0f)
        {
            return paddingEdgeStart.Value;
        }

        var resolvedValue = ComputedEdgeValue(_style.Padding, Leading[axis], YogaValue.Zero).Resolve(widthSize);
        return YogaMath.Max(resolvedValue, 0.0f);
    }

    public double GetTrailingPadding(YogaFlexDirection axis, double? widthSize)
    {
        var paddingEdgeEnd = _style.Padding[YogaEdge.End].Resolve(widthSize);
        if (axis.IsRow()
            && _style.Padding[YogaEdge.End].Unit != YogaUnit.Undefined
            && paddingEdgeEnd != null
            && paddingEdgeEnd >= 0.0f)
        {
            return paddingEdgeEnd.Value;
        }

        var resolvedValue = ComputedEdgeValue(_style.Padding, Trailing[axis], YogaValue.Zero).Resolve(widthSize);
        return YogaMath.Max(resolvedValue, 0.0f);
    }

    public double GetLeadingPaddingAndBorder(YogaFlexDirection axis, double? widthSize)
    {
        return GetLeadingPadding(axis, widthSize) + GetLeadingBorder(axis);
    }

    public double GetTrailingPaddingAndBorder(YogaFlexDirection axis, double? widthSize)
    {
        return GetTrailingPadding(axis, widthSize) + GetTrailingBorder(axis);
    }

    public double ResolveFlexGrow()
    {
        // Root nodes flexGrow should always be 0
        if (_owner == null)
            return 0.0f;

        if (_style.FlexGrow != null)
            return _style.FlexGrow.Value;

        if (_style.Flex != null && _style.Flex.Value > 0.0f)
            return _style.Flex.Value;

        return DefaultFlexGrow;
    }

    public double ResolveFlexShrink()
    {
        if (_owner == null)
            return 0.0f;

        if (_style.FlexShrink != null)
            return _style.FlexShrink.Value;

        if (!_config.UseWebDefaults && _style.Flex != null && _style.Flex.Value < 0.0f)
            return -_style.Flex.Value;

        return _config.UseWebDefaults ? WebDefaultFlexShrink : DefaultFlexShrink;
    }

    public YogaValue ResolveFlexBasis()
    {
        var flexBasis = _style.FlexBasis;
        if (flexBasis.Unit != YogaUnit.Auto && flexBasis.Unit != YogaUnit.Undefined)
            return flexBasis;

        if (_style.Flex != null && _style.Flex.Value > 0.0f)
            return _config.UseWebDefaults ? YogaValue.Auto : YogaValue.Zero;

        return YogaValue.Auto;
    }

    public override string ToString() => Name;

    public void ResolveDimension()
    {
        for (var dim = (int)YogaDimension.Width; dim < 2; dim++)
        {
            if (_style.MaxDimensions[dim].Unit != YogaUnit.Undefined && _style.MaxDimensions[dim].Equals(_style.MinDimensions[dim]))
                _resolvedDimensions[dim] = _style.MaxDimensions[dim];
            else
                _resolvedDimensions[dim] = _style.Dimensions[dim];
        }
    }

    public YogaDirection ResolveDirection(YogaDirection ownerDirection)
    {
        if (_style.Direction == YogaDirection.Inherit)
            return ownerDirection > YogaDirection.Inherit ? ownerDirection : YogaDirection.LeftToRight;

        return _style.Direction;
    }

    public bool IsNodeFlexible()
    {
        return ((_style.PositionType == YogaPositionType.Relative) && (ResolveFlexGrow() != 0 || ResolveFlexShrink() != 0));
    }

    public bool DidUseLegacyFlag()
    {
        var didUseLegacyFlag = _layout.DidUseLegacyFlag;
        if (didUseLegacyFlag)
            return true;

        foreach (var child in _children)
        {
            if (child._layout.DidUseLegacyFlag)
                return true;
        }

        return false;
    }

    // setters
    public void SetLayoutMargin(double? margin, YogaEdge index)
    {
        _layout.Margin[index] = margin;
    }

    public void SetLayoutBorder(double border, YogaEdge index)
    {
        _layout.Border[index] = border;
    }

    public void SetLayoutPadding(double padding, YogaEdge index)
    {
        _layout.Padding[index] = padding;
    }

    public void SetLayoutPosition(double? position, YogaEdge index)
    {
        _layout.Position[(int)index] = position;
    }

    public void SetLayoutMeasuredDimension(double? measuredDimension, YogaDimension index)
    {
        _layout.MeasuredDimensions[index] = measuredDimension;
    }

    public void SetLayoutDimension(double? dimension, YogaDimension index)
    {
        _layout.Dimensions[index] = dimension;
    }

    public void SetPosition(YogaDirection direction, double? mainSize, double? crossSize, double? ownerWidth)
    {
        /* Root nodes should be always layouted as LTR, so we don't return negative
         * values. */
        var directionRespectingRoot = _owner != null ? direction : YogaDirection.LeftToRight;
        var mainAxis = _style.FlexDirection.ResolveFlexDirection(directionRespectingRoot);
        var crossAxis = mainAxis.FlexDirectionCross(directionRespectingRoot);

        var relativePositionMain = GetRelativePosition(mainAxis, mainSize);
        var relativePositionCross = GetRelativePosition(crossAxis, crossSize);

        SetLayoutPosition((GetLeadingMargin(mainAxis, ownerWidth) + relativePositionMain), Leading[mainAxis]);
        SetLayoutPosition((GetTrailingMargin(mainAxis, ownerWidth) + relativePositionMain), Trailing[mainAxis]);
        SetLayoutPosition((GetLeadingMargin(crossAxis, ownerWidth) + relativePositionCross), Leading[crossAxis]);
        SetLayoutPosition((GetTrailingMargin(crossAxis, ownerWidth) + relativePositionCross), Trailing[crossAxis]);
    }

    public void SetAndPropogateUseLegacyFlag(bool useLegacyFlag)
    {
        _config.UseLegacyStretchBehaviour = useLegacyFlag;
        foreach (var item in _children)
            item.Config.UseLegacyStretchBehaviour = useLegacyFlag;
    }

    // Other methods

    public int IndexOf(YogaNode node)
    {
        return _children.IndexOf(node);
    }

    public void Clear()
    {
        foreach (var item in _children)
            item._owner = null;

        _children.Clear();
        _isDirty = true;
    }

    public void ReplaceChild(YogaNode oldChild, YogaNode newChild)
    {
        var index = _children.IndexOf(oldChild);
        if (index < 0)
            return;

        newChild.Owner = this;
        _children[index] = newChild;

        MarkDirty();
    }

    public void ReplaceChild(YogaNode child, int index)
    {
        child.Owner = this;
        _children[index] = child;

        MarkDirty();
    }

    public void Insert(int index, YogaNode child)
    {
        if (child.Owner != null)
            throw new InvalidOperationException("Child already has a owner, it must be removed first.");

        if (_measure != null)
            throw new InvalidOperationException("Cannot add child: Nodes with measure functions cannot have children.");

        child.Owner = this;
        _children.Insert(index, child);

        MarkDirty();
    }

    public void Add(YogaNode child)
    {
        if (child.Owner != null)
            throw new InvalidOperationException("Child already has a owner, it must be removed first.");

        if (_measure != null)
            throw new InvalidOperationException("Cannot add child: Nodes with measure functions cannot have children.");

        child.Owner = this;
        _children.Add(child);

        MarkDirty();
    }

    public bool Remove(YogaNode child)
    {
        if (child.Owner == this)
            child.Owner = null;

        var result = _children.Remove(child);
        if (result)
            MarkDirty();

        return result;
    }

    public void RemoveAt(int index)
    {
        var child = _children[index];
        child.Owner = null;
        _children.RemoveAt(index);

        MarkDirty();
    }

    public void MarkDirty()
    {
        if (!_isDirty)
        {
            IsDirty = true;

            Layout.ComputedFlexBasis = default(double?);
            if (_owner != null)
                _owner.MarkDirty();
        }
    }

    public void MarkDirtyAndPropogateDownwards()
    {
        _isDirty = true;
        foreach (var item in _children)
            item.MarkDirtyAndPropogateDownwards();
    }

    public bool IsLayoutTreeEqualToNode(YogaNode node)
    {
        if (_children.Count != node._children.Count)
            return false;

        if (_layout != node._layout)
            return false;

        if (_children.Count == 0)
            return true;

        var isLayoutTreeEqual = true;
        var otherNodeChildren = default(YogaNode);
        for (var i = 0; i < _children.Count; ++i)
        {
            otherNodeChildren = node._children[i];
            isLayoutTreeEqual = _children[i].IsLayoutTreeEqualToNode(otherNodeChildren);
            if (!isLayoutTreeEqual)
                return false;
        }

        return isLayoutTreeEqual;
    }

    private void SetChildTrailingPosition(YogaNode child, YogaFlexDirection axis)
    {
        var size = child.Layout.MeasuredDimensions[Dimension[axis]];
        child.SetLayoutPosition(Layout.MeasuredDimensions[Dimension[axis]] - size - child.Layout.Position[Position[axis]], Trailing[axis]);
    }

    private void CloneChildrenIfNeeded()
    {
        var childCount = _children.Count;
        if (childCount == 0)
            return;

        var firstChild = _children[0];
        if (firstChild.Owner == this)
        {
            // If the first child has this node as its owner, we assume that it is
            // already unique. We can do this because if we have it as a child, that
            // means that its owner was at some point cloned which made that subtree
            // immutable. We also assume that all its sibling are cloned as well.
            return;
        }

        var cloneNodeCallback = _config.OnNodeCloned;
        for (var i = 0; i < childCount; ++i)
        {
            var oldChild = _children[i];
            var newChild = new YogaNode(oldChild)
            {
                _owner = null,
                ParentLayout = ParentLayout
            };

            ReplaceChild(newChild, i);
            newChild.Owner = this;
            if (cloneNodeCallback != null)
                cloneNodeCallback(oldChild, newChild, this, i);
        }
    }

    private static double? ResolveValueMargin(YogaValue value, double? ownerSize)
    {
        return value.Unit == YogaUnit.Auto ? 0F : value.Resolve(ownerSize);
    }

    /// <summary>
    /// Finds a node in the layout tree by a query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <example>content #left article</example>
    public YogaNode? FindNode(string query)
    {
        var parts = query.Split(' ');
        var currentNode = this;

        foreach (var part in parts)
        {
            currentNode = currentNode.Children.FirstOrDefault(child =>
            {
                if (part.StartsWith('#'))
                {
                    return child.ID == part[1..];
                }

                return child.Name == part;
            });
            if (currentNode == null)
            {
                return null;
            }
        }

        return currentNode;
    }

    public IEnumerable<YogaNode> FindNodes(string query)
    {
        var parts = query.Split(' ');
        var currentNodes = new List<YogaNode> { this };

        foreach (var part in parts)
        {
            currentNodes = currentNodes
                .SelectMany(node => node.Children)
                .Where(child =>
                {
                    if (part.StartsWith('#'))
                    {
                        return child.ID == part[1..];
                    }

                    return child.Name == part;
                })
                .ToList();
        }

        return currentNodes;
    }

    /// <summary>
    /// Finds a node in the layout tree by a query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <example>content #left article</example>
    public T? FindNode<T>(string query)
        where T : YogaNode
    {
        return (T?)FindNode(query);
    }

    public IEnumerable<T?> FindNodes<T>(string query)
        where T : YogaNode
    {
        return (IEnumerable<T?>)FindNodes(query);
    }

    private static YogaValue ComputedEdgeValue(YogaArray<YogaValue> edges, YogaEdge edge, YogaValue defaultValue)
    {
        if (edges[edge].Unit != YogaUnit.Undefined)
            return edges[edge];

        if ((edge == YogaEdge.Top || edge == YogaEdge.Bottom) && edges[YogaEdge.Vertical].Unit != YogaUnit.Undefined)
            return edges[YogaEdge.Vertical];

        if ((edge == YogaEdge.Left || edge == YogaEdge.Right || edge == YogaEdge.Start || edge == YogaEdge.End) && edges[YogaEdge.Horizontal].Unit != YogaUnit.Undefined)
            return edges[YogaEdge.Horizontal];

        if (edges[YogaEdge.All].Unit != YogaUnit.Undefined)
            return edges[YogaEdge.All];

        if (edge == YogaEdge.Start || edge == YogaEdge.End)
            return YogaValue.Unset;

        return defaultValue;
    }

    public virtual void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        if (Display == YogaDisplay.None)
        {
            return;
        }

        // draw box shadow
        if (BoxShadow != null)
        {
            DrawBoxShadow(page, absoluteX, absoluteY);
        }

        if (Background != null)
        {
            page.SetStrokeColor(Background.r, Background.g, Background.b);
            page.SetTextAndFillColor(Background.r, Background.g, Background.b);
            var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
            page.DrawRectangle(boxPos, LayoutWidth, LayoutHeight, 0, true);
            page.ResetColor();
        }

        if (BorderColor != null)
        {
            page.SetStrokeColor(BorderColor.r, BorderColor.g, BorderColor.b);

            var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
            page.DrawRectangle(boxPos, LayoutWidth, LayoutHeight, 1);
            page.ResetColor();
        }
    }

    private void DrawBoxShadow(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        for (int i = 0; i < 5; i++)
        {
            double offset = BoxShadow.Offset * (i + 1) / 5.0;

            var r = (byte)Math.Min(BoxShadow.Color.r + i * 20, 255);
            var g = (byte)Math.Min(BoxShadow.Color.g + i * 20, 255);
            var b = (byte)Math.Min(BoxShadow.Color.b + i * 20, 255);

            page.SetStrokeColor(r, g, b);
            page.SetTextAndFillColor(r, g, b);

            var shadowPos = new PdfPoint(
                absoluteX + offset,
                page.PageSize.Height - absoluteY - LayoutHeight - offset
            );

            page.DrawRectangle(
                shadowPos,
                LayoutWidth + offset * 2,
                LayoutHeight + offset * 2,
                0,
                true
            );
        }

        page.ResetColor();
    }

    public virtual void ReCalculate(PdfPageBuilder page)
    {
        if (Gap.Unit != YogaUnit.Undefined)
        {
            SetChildGaps();
        }
    }

    private void SetChildGaps()
    {
        for (int i = 0; i < Children.Count - 1; i++)
        {
            var child = Children[i];

            if (FlexDirection == YogaFlexDirection.Column)
            {
                child.MarginBottom = Gap;
            }
            else if(FlexDirection == YogaFlexDirection.Row)
            {
                child.MarginRight = Gap;
            }
        }
    }

    public void SetAttributes(XElement element)
    {
        foreach (var attr in element.Attributes())
        {
            switch (attr.Name.LocalName.ToLower())
            {
                case "height":
                    Height = YogaValue.Parse(attr.Value);
                    break;
                case "width":
                    Width = YogaValue.Parse(attr.Value);
                    break;
                case "id":
                    ID = attr.Value;
                    break;
                case "margintop":
                    MarginTop = YogaValue.Parse(attr.Value);
                    break;
                case "gap":
                    Gap = YogaValue.Parse(attr.Value);
                    break;
                case "marginbottom":
                    MarginBottom = YogaValue.Parse(attr.Value);
                    break;
                case "margin":
                    Margin = YogaValue.Parse(attr.Value);
                    break;
                case "marginright":
                    MarginRight = YogaValue.Parse(attr.Value);
                    break;
                case "marginleft":
                    MarginLeft = YogaValue.Parse(attr.Value);
                    break;

                case "padding":
                    Padding = YogaValue.Parse(attr.Value);
                    break;
                case "paddingtop":
                    PaddingTop = YogaValue.Parse(attr.Value);
                    break;
                case "paddingbottom":
                    PaddingBottom = YogaValue.Parse(attr.Value);
                    break;
                case "paddingright":
                    PaddingRight = YogaValue.Parse(attr.Value);
                    break;
                case "paddingleft":
                    PaddingLeft = YogaValue.Parse(attr.Value);
                    break;
                case "flexgrow":
                    FlexGrow = double.Parse(attr.Value, CultureInfo.InvariantCulture);
                    break;
                case "background":
                    Background = Colors.Parse(attr.Value);
                    break;
                case "alignitems":
                    AlignItems = Enum.Parse<YogaAlign>(attr.Value, true);
                    break;
                case "aligncontent":
                    AlignContent = Enum.Parse<YogaAlign>(attr.Value, true);
                    break;
                case "justifycontent":
                    JustifyContent = Enum.Parse<YogaJustify>(attr.Value, true);
                    break;
                case "display":
                    Display = Enum.Parse<YogaDisplay>(attr.Value, true);
                    break;
                case "flexdirection":
                    FlexDirection = Enum.Parse<YogaFlexDirection>(attr.Value, true);
                    break;
                case "bordercolor":
                    BorderColor = Colors.Parse(attr.Value);
                    break;
                case "boxshadow":
                    var spl = attr.Value.Split(' ');
                    BoxShadow = new BoxShadow(Colors.Parse(spl[0]), int.Parse(spl[1]));
                    break;
                case "positiontype":
                    PositionType = Enum.Parse<YogaPositionType>(attr.Value, true);
                    break;
                case "alignself":
                    AlignSelf = Enum.Parse<YogaAlign>(attr.Value, true);
                    break;
                default:
                    SetAttribute(attr.Name.LocalName.ToLower(), attr.Value);
                    break;
            }
        }
    }

    internal virtual void SetAttribute(string name, string value)
    {

    }
}