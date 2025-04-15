/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Moss.NET.Sdk.LayoutEngine;

public struct YogaSize
{
    public double? Width;
    public double? Height;

    public static YogaSize From(double? width, double? height)
    {
        return new YogaSize()
        {
            Width = width,
            Height = height,
        };
    }
}