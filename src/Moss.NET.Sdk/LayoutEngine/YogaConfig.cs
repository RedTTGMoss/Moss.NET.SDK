/**
 * Copyright (c) 2014-present, Facebook, Inc.
 * Copyright (c) 2018-present, Marius Klimantavičius
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Moss.NET.Sdk.LayoutEngine;

public class YogaConfig
{
    public YogaConfig()
    {
    }

    public YogaConfig(YogaConfig oldConfig)
    {
        ExperimentalFeatures = new bool[oldConfig.ExperimentalFeatures.Length];
        Array.Copy(oldConfig.ExperimentalFeatures, ExperimentalFeatures, ExperimentalFeatures.Length);

        UseWebDefaults = oldConfig.UseWebDefaults;
        UseLegacyStretchBehaviour = oldConfig.UseLegacyStretchBehaviour;
        ShouldDiffLayoutWithoutLegacyStretchBehaviour = oldConfig.ShouldDiffLayoutWithoutLegacyStretchBehaviour;
        PointScaleFactor = oldConfig.PointScaleFactor;
        OnNodeCloned = oldConfig.OnNodeCloned;
    }

    public bool[] ExperimentalFeatures { get; set; } = new[] { false };

    public bool UseWebDefaults { get; set; }

    public bool UseLegacyStretchBehaviour { get; set; }

    public bool ShouldDiffLayoutWithoutLegacyStretchBehaviour { get; set; }

    public double PointScaleFactor { get; set; } = 1.0F;

    public YogaNodeCloned OnNodeCloned { get; set; }

    public bool IsExperimentalFeatureEnabled(YogaExperimentalFeature feature)
    {
        return ExperimentalFeatures[(int)feature];
    }

    public YogaConfig DeepClone()
    {
        return new YogaConfig(this);
    }
}