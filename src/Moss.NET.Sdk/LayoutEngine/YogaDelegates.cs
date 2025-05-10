/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Moss.NET.Sdk.LayoutEngine;

public delegate YogaSize YogaMeasure(YogaNode node, double? width, YogaMeasureMode widthMode, double? height,
    YogaMeasureMode heightMode);

public delegate double YogaBaseline(YogaNode node, double? width, double? height);

public delegate void YogaDirtied(YogaNode node);

public delegate void YogaPrint(YogaNode node);

public delegate void YogaNodeCloned(YogaNode oldNode, YogaNode newNode, YogaNode owner, int childIndex);