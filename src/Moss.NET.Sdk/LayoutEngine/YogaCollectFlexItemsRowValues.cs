/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Moss.NET.Sdk.LayoutEngine;

public class YogaCollectFlexItemsRowValues
{
    public int ItemsOnLine;
    public double? SizeConsumedOnCurrentLine = 0;
    public double? TotalFlexGrowFactors = 0;
    public double? TotalFlexShrinkScaledFactors = 0;
    public int EndOfLineIndex;
    public List<YogaNode> RelativeChildren = new List<YogaNode>();
    public double? RemainingFreeSpace = 0;
    // The size of the mainDim for the row after considering size, padding, margin
    // and border of flex items. This is used to calculate maxLineDim after going
    // through all the rows to decide on the main axis size of owner.
    public double? MainDimension = 0;
    // The size of the crossDim for the row after considering size, padding,
    // margin and border of flex items. Used for calculating containers crossSize.
    public double? CrossDimension = 0;
}