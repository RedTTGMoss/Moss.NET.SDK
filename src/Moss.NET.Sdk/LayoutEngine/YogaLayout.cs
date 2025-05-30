﻿/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Moss.NET.Sdk.LayoutEngine;

public sealed class YogaLayout
{
    public const int MaxCachedResultCount = 16;

    public static readonly double?[] DefaultDimensionValues = new[] { default(double?), default(double?) };
    public YogaArray<double> Border; // 6

    public YogaCachedMeasurement CachedLayout;
    public YogaCachedMeasurement[] CachedMeasurements; // MaxCachedResultCount
    public double? ComputedFlexBasis;

    public uint ComputedFlexBasisGeneration;
    public bool DidUseLegacyFlag;
    public YogaArray<double?> Dimensions; // 2
    public YogaDirection Direction;
    public bool DoesLegacyStretchFlagAffectsLayout;

    // Instead of recomputing the entire layout every single time, we
    // cache some information to break early when nothing changed
    public uint GenerationCount;
    public bool HadOverflow;
    public YogaDirection LastOwnerDirection;
    public YogaArray<double?> Margin; // 6
    public YogaArray<double?> MeasuredDimensions; // 2

    public int NextCachedMeasurementsIndex;
    public YogaArray<double> Padding; // 6

    public YogaArray<double?> Position; // 4

    public YogaLayout()
    {
        var cached = new YogaCachedMeasurement[MaxCachedResultCount];
        for (var i = 0; i < cached.Length; i++)
            cached[i] = new YogaCachedMeasurement();

        Position = new YogaArray<double?>(4);
        Dimensions = YogaArray.From(DefaultDimensionValues);
        Margin = new YogaArray<double?>(6);
        Border = new YogaArray<double>(6);
        Padding = new YogaArray<double>(6);
        Direction = YogaDirection.Inherit;
        ComputedFlexBasisGeneration = 0;
        ComputedFlexBasis = null;
        HadOverflow = false;
        GenerationCount = 0;
        LastOwnerDirection = (YogaDirection)(-1);
        NextCachedMeasurementsIndex = 0;
        CachedMeasurements = cached;
        MeasuredDimensions = YogaArray.From(DefaultDimensionValues);
        CachedLayout = new YogaCachedMeasurement();
        DidUseLegacyFlag = false;
        DoesLegacyStretchFlagAffectsLayout = false;
    }

    public static bool operator ==(YogaLayout self, YogaLayout layout)
    {
        if (ReferenceEquals(self, layout))
            return true;

        if (ReferenceEquals(self, null) || ReferenceEquals(layout, null))
            return false;

        var isEqual = YogaArray.Equal(self.Position, layout.Position)
                      && YogaArray.Equal(self.Dimensions, layout.Dimensions)
                      && YogaArray.Equal(self.Margin, layout.Margin)
                      && YogaArray.Equal(self.Border, layout.Border)
                      && YogaArray.Equal(self.Padding, layout.Padding)
                      && self.Direction == layout.Direction
                      && self.HadOverflow == layout.HadOverflow
                      && self.LastOwnerDirection == layout.LastOwnerDirection
                      && self.NextCachedMeasurementsIndex == layout.NextCachedMeasurementsIndex
                      && self.CachedLayout == layout.CachedLayout;

        for (var i = 0; i < MaxCachedResultCount && isEqual; ++i)
            isEqual = isEqual && self.CachedMeasurements[i] == layout.CachedMeasurements[i];

        isEqual = isEqual && self.ComputedFlexBasis == layout.ComputedFlexBasis;
        isEqual = isEqual && self.MeasuredDimensions[0] == layout.MeasuredDimensions[0];
        isEqual = isEqual && self.MeasuredDimensions[1] == layout.MeasuredDimensions[1];

        return isEqual;
    }

    public static bool operator !=(YogaLayout self, YogaLayout layout)
    {
        return !(self == layout);
    }

    public void CopyFrom(YogaLayout other)
    {
        Position.CopyFrom(other.Position);
        Dimensions.CopyFrom(other.Dimensions);
        Margin.CopyFrom(other.Margin);
        Border.CopyFrom(other.Border);
        Padding.CopyFrom(other.Padding);
        Direction = other.Direction;
        ComputedFlexBasisGeneration = other.ComputedFlexBasisGeneration;
        ComputedFlexBasis = other.ComputedFlexBasis;
        HadOverflow = other.HadOverflow;
        GenerationCount = other.GenerationCount;
        LastOwnerDirection = other.LastOwnerDirection;
        NextCachedMeasurementsIndex = other.NextCachedMeasurementsIndex;
        Array.Copy(other.CachedMeasurements, CachedMeasurements, CachedMeasurements.Length);
        MeasuredDimensions.CopyFrom(other.MeasuredDimensions);
        CachedLayout.CopyFrom(other.CachedLayout);
        DidUseLegacyFlag = other.DidUseLegacyFlag;
        DoesLegacyStretchFlagAffectsLayout = other.DoesLegacyStretchFlagAffectsLayout;
    }

    public void Clear()
    {
        Position.Clear();
        Dimensions.Clear();
        Margin.Clear();
        Border.Clear();
        Padding.Clear();
        Direction = YogaDirection.Inherit;
        ComputedFlexBasisGeneration = 0;
        ComputedFlexBasis = 0F;
        HadOverflow = false;
        GenerationCount = 0;
        LastOwnerDirection = YogaDirection.Inherit;
        NextCachedMeasurementsIndex = 0;
        MeasuredDimensions.Clear();
        CachedLayout.Clear();
        DidUseLegacyFlag = false;
        DoesLegacyStretchFlagAffectsLayout = false;

        foreach (var item in CachedMeasurements)
            item.Clear();
    }

    public override bool Equals(object obj)
    {
        var other = obj as YogaLayout;
        return other == this;
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode() +
               Dimensions.GetHashCode() +
               Margin.GetHashCode() +
               Border.GetHashCode() +
               Padding.GetHashCode() +
               Direction.GetHashCode() +
               ComputedFlexBasisGeneration.GetHashCode() +
               ComputedFlexBasis.GetHashCode() +
               HadOverflow.GetHashCode() +
               GenerationCount.GetHashCode() +
               LastOwnerDirection.GetHashCode() +
               NextCachedMeasurementsIndex.GetHashCode() +
               CachedMeasurements.GetHashCode() +
               MeasuredDimensions.GetHashCode() +
               CachedLayout.GetHashCode() +
               DidUseLegacyFlag.GetHashCode() +
               DoesLegacyStretchFlagAffectsLayout.GetHashCode();
    }
}