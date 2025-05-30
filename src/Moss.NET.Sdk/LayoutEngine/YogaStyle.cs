﻿/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Moss.NET.Sdk.LayoutEngine;

public sealed class YogaStyle
{
    private static readonly YogaValue[] DefaultEdgeValuesUnit = new[]
    {
        YogaValue.Unset,
        YogaValue.Unset,
        YogaValue.Unset,
        YogaValue.Unset,
        YogaValue.Unset,
        YogaValue.Unset,
        YogaValue.Unset,
        YogaValue.Unset,
        YogaValue.Unset
    };

    private static readonly YogaValue[] DefaultDimensionValuesAutoUnit = new[] { YogaValue.Auto, YogaValue.Auto };
    private static readonly YogaValue[] DefaultDimensionValuesUnit = new[] { YogaValue.Unset, YogaValue.Unset };
    public YogaAlign AlignContent;
    public YogaAlign AlignItems;
    public YogaAlign AlignSelf;
    public double? AspectRatio;
    public YogaArray<YogaValue> Border; // YGEdgeCount
    public YogaArray<YogaValue> Dimensions; // 2

    public YogaDirection Direction;
    public YogaDisplay Display;
    public double? Flex;
    public YogaValue FlexBasis;
    public YogaFlexDirection FlexDirection;
    public double? FlexGrow;
    public double? FlexShrink;
    public YogaWrap FlexWrap;
    public YogaJustify JustifyContent;
    public YogaArray<YogaValue> Margin; // YGEdgeCount
    public YogaArray<YogaValue> MaxDimensions; // 2
    public YogaArray<YogaValue> MinDimensions; // 2
    public YogaOverflow Overflow;
    public YogaArray<YogaValue> Padding; // YGEdgeCount
    public YogaArray<YogaValue> Position; // YGEdgeCount
    public YogaPositionType PositionType;

    public YogaStyle()
    {
        Direction = YogaDirection.Inherit;
        FlexDirection = YogaFlexDirection.Column;
        JustifyContent = YogaJustify.FlexStart;
        AlignContent = YogaAlign.FlexStart;
        AlignItems = YogaAlign.Stretch;
        AlignSelf = YogaAlign.Auto;
        PositionType = YogaPositionType.Relative;
        FlexWrap = YogaWrap.NoWrap;
        Overflow = YogaOverflow.Visible;
        Display = YogaDisplay.Flex;
        Flex = null;
        FlexGrow = null;
        FlexShrink = null;
        FlexBasis = YogaValue.Auto;
        Margin = YogaArray.From(DefaultEdgeValuesUnit);
        Position = YogaArray.From(DefaultEdgeValuesUnit);
        Padding = YogaArray.From(DefaultEdgeValuesUnit);
        Border = YogaArray.From(DefaultEdgeValuesUnit);
        Dimensions = YogaArray.From(DefaultDimensionValuesAutoUnit);
        MinDimensions = YogaArray.From(DefaultDimensionValuesUnit);
        MaxDimensions = YogaArray.From(DefaultDimensionValuesUnit);
        AspectRatio = null;
    }

    public void CopyFrom(YogaStyle other)
    {
        Direction = other.Direction;
        FlexDirection = other.FlexDirection;
        JustifyContent = other.JustifyContent;
        AlignContent = other.AlignContent;
        AlignItems = other.AlignItems;
        AlignSelf = other.AlignSelf;
        PositionType = other.PositionType;
        FlexWrap = other.FlexWrap;
        Overflow = other.Overflow;
        Display = other.Display;
        Flex = other.Flex;
        FlexGrow = other.FlexGrow;
        FlexShrink = other.FlexShrink;
        FlexBasis = other.FlexBasis;
        Margin.CopyFrom(other.Margin);
        Position.CopyFrom(other.Position);
        Padding.CopyFrom(other.Padding);
        Border.CopyFrom(other.Border);
        Dimensions.CopyFrom(other.Dimensions);
        MinDimensions.CopyFrom(other.MinDimensions);
        MaxDimensions.CopyFrom(other.MaxDimensions);
        AspectRatio = other.AspectRatio;
    }

    // Yoga specific properties, not compatible with flexbox specification
    public static bool operator ==(YogaStyle self, YogaStyle style)
    {
        if (ReferenceEquals(self, style))
            return true;

        if (ReferenceEquals(self, null) || ReferenceEquals(style, null))
            return false;

        var areNondoubleValuesEqual =
            self.Direction == style.Direction &&
            self.FlexDirection == style.FlexDirection &&
            self.JustifyContent == style.JustifyContent &&
            self.AlignContent == style.AlignContent &&
            self.AlignItems == style.AlignItems &&
            self.AlignSelf == style.AlignSelf &&
            self.PositionType == style.PositionType &&
            self.FlexWrap == style.FlexWrap &&
            self.Overflow == style.Overflow &&
            self.Display == style.Display &&
            self.FlexBasis.Equals(style.FlexBasis) &&
            YogaArray.Equal(self.Margin, style.Margin) &&
            YogaArray.Equal(self.Position, style.Position) &&
            YogaArray.Equal(self.Padding, style.Padding) &&
            YogaArray.Equal(self.Border, style.Border) &&
            YogaArray.Equal(self.Dimensions, style.Dimensions) &&
            YogaArray.Equal(self.MinDimensions, style.MinDimensions) &&
            YogaArray.Equal(self.MaxDimensions, style.MaxDimensions);

        areNondoubleValuesEqual = areNondoubleValuesEqual && self.Flex == style.Flex;

        areNondoubleValuesEqual = areNondoubleValuesEqual && self.FlexGrow == style.FlexGrow;
        areNondoubleValuesEqual = areNondoubleValuesEqual && self.FlexShrink == style.FlexShrink;
        areNondoubleValuesEqual = areNondoubleValuesEqual && self.AspectRatio == style.AspectRatio;

        return areNondoubleValuesEqual;
    }

    public static bool operator !=(YogaStyle self, YogaStyle style)
    {
        return !(self == style);
    }

    public override bool Equals(object obj)
    {
        var other = obj as YogaStyle;
        return other == this;
    }

    public override int GetHashCode()
    {
        return Direction.GetHashCode() +
               FlexDirection.GetHashCode() +
               JustifyContent.GetHashCode() +
               AlignContent.GetHashCode() +
               AlignItems.GetHashCode() +
               AlignSelf.GetHashCode() +
               PositionType.GetHashCode() +
               FlexWrap.GetHashCode() +
               Overflow.GetHashCode() +
               Display.GetHashCode() +
               Flex.GetHashCode() +
               FlexGrow.GetHashCode() +
               FlexShrink.GetHashCode() +
               FlexBasis.GetHashCode() +
               Margin.GetHashCode() +
               Position.GetHashCode() +
               Padding.GetHashCode() +
               Border.GetHashCode() +
               Dimensions.GetHashCode() +
               MinDimensions.GetHashCode() +
               MaxDimensions.GetHashCode() +
               AspectRatio.GetHashCode();
    }
}