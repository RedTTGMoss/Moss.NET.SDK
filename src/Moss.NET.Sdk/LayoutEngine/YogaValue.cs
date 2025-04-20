/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Globalization;

namespace Moss.NET.Sdk.LayoutEngine;

public struct YogaValue: IEquatable<YogaValue>
{
    public static readonly YogaValue Zero = new YogaValue { Value = 0, Unit = YogaUnit.Point };
    public static readonly YogaValue Unset = new YogaValue { Value = null, Unit = YogaUnit.Undefined };
    public static readonly YogaValue Auto = new YogaValue { Value = null, Unit = YogaUnit.Auto };

    public double? Value;
    public YogaUnit Unit;

    public static YogaValue Parse(string value)
    {
        if (value == "auto")
        {
            return Auto;
        }

        if (value == "unset")
        {
            return Unset;
        }

        var unit = YogaUnit.Point;
        if (value.EndsWith('%'))
        {
            value = value.TrimEnd('%');
            unit = YogaUnit.Percent;
        }

        var number = double.Parse(value.Trim(), CultureInfo.InvariantCulture);

        return new YogaValue { Value = number, Unit = unit };
    }

    public override string ToString()
    {
        if (Unit == YogaUnit.Undefined)
            return "undefined";

        if (Unit == YogaUnit.Auto)
            return "auto";

        if (Unit == YogaUnit.Point)
            return $"{Value} pt";

        return $"{Value} %";
    }

    public double? Resolve(double? ownerSize)
    {
        switch (Unit)
        {
            case YogaUnit.Undefined:
            case YogaUnit.Auto:
                return null;
            case YogaUnit.Point:
                return Value;
            case YogaUnit.Percent:
                return Value * ownerSize * 0.01f;
        }

        return null;
    }

    public static YogaValue Percent(double percentValue)
    {
        return new YogaValue() { Unit = YogaUnit.Percent, Value = percentValue };
    }

    public static YogaValue Point(double pointValue)
    {
        return new YogaValue() { Unit = YogaUnit.Point, Value = pointValue };
    }

    public bool Equals(YogaValue b)
    {
        if (Unit != b.Unit)
            return false;

        if (Unit == YogaUnit.Undefined || (Value == null && b.Value == null))
            return true;

        if (Value == null && b.Value == null)
            return true;

        if (Value == null || b.Value == null)
            return false;

        return Math.Abs(Value.Value - b.Value.Value) < 0.0001f;
    }

    public static implicit operator YogaValue(double pointValue)
    {
        return Point(pointValue);
    }
}