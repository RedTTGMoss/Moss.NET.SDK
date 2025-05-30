﻿/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Moss.NET.Sdk.LayoutEngine;

public sealed class YogaCachedMeasurement
{
    public double? AvailableHeight;
    public double? AvailableWidth;
    public double? ComputedHeight;

    public double? ComputedWidth;
    public YogaMeasureMode HeightMeasureMode;
    public YogaMeasureMode WidthMeasureMode;

    public YogaCachedMeasurement()
    {
        AvailableWidth = 0;
        AvailableHeight = 0;
        WidthMeasureMode = (YogaMeasureMode)(-1);
        HeightMeasureMode = (YogaMeasureMode)(-1);
        ComputedWidth = -1;
        ComputedHeight = -1;
    }

    public static bool operator ==(YogaCachedMeasurement self, YogaCachedMeasurement measurement)
    {
        if (ReferenceEquals(self, measurement))
            return true;

        if (ReferenceEquals(self, null) || ReferenceEquals(measurement, null))
            return false;

        var isEqual = self.WidthMeasureMode == measurement.WidthMeasureMode &&
                      self.HeightMeasureMode == measurement.HeightMeasureMode;

        isEqual = isEqual && self.AvailableWidth == measurement.AvailableWidth;
        isEqual = isEqual && self.AvailableHeight == measurement.AvailableHeight;
        isEqual = isEqual && self.ComputedWidth == measurement.ComputedWidth;
        isEqual = isEqual && self.ComputedHeight == measurement.ComputedHeight;

        return isEqual;
    }

    public static bool operator !=(YogaCachedMeasurement self, YogaCachedMeasurement measurement)
    {
        return !(self == measurement);
    }

    public void CopyFrom(YogaCachedMeasurement other)
    {
        AvailableWidth = other.AvailableWidth;
        AvailableHeight = other.AvailableHeight;
        WidthMeasureMode = other.WidthMeasureMode;
        HeightMeasureMode = other.HeightMeasureMode;
        ComputedWidth = other.ComputedWidth;
        ComputedHeight = other.ComputedHeight;
    }

    public void Clear()
    {
        AvailableWidth = 0;
        AvailableHeight = 0;
        WidthMeasureMode = (YogaMeasureMode)(-1);
        HeightMeasureMode = (YogaMeasureMode)(-1);
        ComputedWidth = -1;
        ComputedHeight = -1;
    }

    public override bool Equals(object obj)
    {
        var other = obj as YogaCachedMeasurement;
        return other == this;
    }

    public override int GetHashCode()
    {
        return AvailableWidth.GetHashCode() +
               AvailableHeight.GetHashCode() +
               WidthMeasureMode.GetHashCode() +
               HeightMeasureMode.GetHashCode() +
               ComputedWidth.GetHashCode() +
               ComputedHeight.GetHashCode();
    }
}