﻿using System.Globalization;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk.UI;

public readonly struct Color(long r, long g, long b, long? a = 255)
{
    [JsonPropertyName("r")] public long R { get; init; } = r;

    [JsonPropertyName("g")] public long G { get; init; } = g;

    [JsonPropertyName("b")] public long B { get; init; } = b;

    [JsonPropertyName("a")] public long? A { get; init; } = a;

    public static Color AliceBlue => new(240, 248, 255);
    public static Color AntiqueWhite => new(250, 235, 215);
    public static Color Aqua => new(0, 255, 255);
    public static Color Aquamarine => new(127, 255, 212);
    public static Color Azure => new(240, 255, 255);
    public static Color Beige => new(245, 245, 220);
    public static Color Bisque => new(255, 228, 196);
    public static Color Black => new(0, 0, 0);
    public static Color BlanchedAlmond => new(255, 235, 205);
    public static Color Blue => new(0, 0, 255);
    public static Color BlueViolet => new(138, 43, 226);
    public static Color Brown => new(165, 42, 42);
    public static Color BurlyWood => new(222, 184, 135);
    public static Color CadetBlue => new(95, 158, 160);
    public static Color Chartreuse => new(127, 255, 0);
    public static Color Chocolate => new(210, 105, 30);
    public static Color Coral => new(255, 127, 80);
    public static Color CornflowerBlue => new(100, 149, 237);
    public static Color Cornsilk => new(255, 248, 220);
    public static Color Crimson => new(220, 20, 60);
    public static Color Cyan => new(0, 255, 255);
    public static Color DarkBlue => new(0, 0, 139);
    public static Color DarkCyan => new(0, 139, 139);
    public static Color DarkGoldenrod => new(184, 134, 11);
    public static Color DarkGray => new(169, 169, 169);
    public static Color DarkGreen => new(0, 100, 0);
    public static Color DarkKhaki => new(189, 183, 107);
    public static Color DarkMagenta => new(139, 0, 139);
    public static Color DarkOliveGreen => new(85, 107, 47);
    public static Color DarkOrange => new(255, 140, 0);
    public static Color DarkOrchid => new(153, 50, 204);
    public static Color DarkRed => new(139, 0, 0);
    public static Color DarkSalmon => new(233, 150, 122);
    public static Color DarkSeaGreen => new(143, 188, 143);
    public static Color DarkSlateBlue => new(72, 61, 139);
    public static Color DarkSlateGray => new(47, 79, 79);
    public static Color DarkTurquoise => new(0, 206, 209);
    public static Color DarkViolet => new(148, 0, 211);
    public static Color DeepPink => new(255, 20, 147);
    public static Color DeepSkyBlue => new(0, 191, 255);
    public static Color DimGray => new(105, 105, 105);
    public static Color DodgerBlue => new(30, 144, 255);
    public static Color Firebrick => new(178, 34, 34);
    public static Color FloralWhite => new(255, 250, 240);
    public static Color ForestGreen => new(34, 139, 34);
    public static Color Fuchsia => new(255, 0, 255);
    public static Color Gainsboro => new(220, 220, 220);
    public static Color GhostWhite => new(248, 248, 255);
    public static Color Gold => new(255, 215, 0);
    public static Color Goldenrod => new(218, 165, 32);
    public static Color Gray => new(128, 128, 128);
    public static Color Green => new(0, 128, 0);
    public static Color GreenYellow => new(173, 255, 47);
    public static Color Honeydew => new(240, 255, 240);
    public static Color HotPink => new(255, 105, 180);
    public static Color IndianRed => new(205, 92, 92);
    public static Color Indigo => new(75, 0, 130);
    public static Color Ivory => new(255, 255, 240);
    public static Color Khaki => new(240, 230, 140);
    public static Color Lavender => new(230, 230, 250);
    public static Color LavenderBlush => new(255, 240, 245);
    public static Color LawnGreen => new(124, 252, 0);
    public static Color LemonChiffon => new(255, 250, 205);
    public static Color LightBlue => new(173, 216, 230);
    public static Color LightCoral => new(240, 128, 128);
    public static Color LightCyan => new(224, 255, 255);
    public static Color LightGoldenrodYellow => new(250, 250, 210);
    public static Color LightGray => new(211, 211, 211);
    public static Color LightGreen => new(144, 238, 144);
    public static Color LightPink => new(255, 182, 193);
    public static Color LightSalmon => new(255, 160, 122);
    public static Color LightSeaGreen => new(32, 178, 170);
    public static Color LightSkyBlue => new(135, 206, 250);
    public static Color LightSlateGray => new(119, 136, 153);
    public static Color LightSteelBlue => new(176, 196, 222);
    public static Color LightYellow => new(255, 255, 224);
    public static Color Lime => new(0, 255, 0);
    public static Color LimeGreen => new(50, 205, 50);
    public static Color Linen => new(250, 240, 230);
    public static Color Magenta => new(255, 0, 255);
    public static Color Maroon => new(128, 0, 0);
    public static Color MediumAquamarine => new(102, 205, 170);
    public static Color MediumBlue => new(0, 0, 205);
    public static Color MediumOrchid => new(186, 85, 211);
    public static Color MediumPurple => new(147, 112, 219);
    public static Color MediumSeaGreen => new(60, 179, 113);
    public static Color MediumSlateBlue => new(123, 104, 238);
    public static Color MediumSpringGreen => new(0, 250, 154);
    public static Color MediumTurquoise => new(72, 209, 204);
    public static Color MediumVioletRed => new(199, 21, 133);
    public static Color MidnightBlue => new(25, 25, 112);
    public static Color MintCream => new(245, 255, 250);
    public static Color MistyRose => new(255, 228, 225);
    public static Color Moccasin => new(255, 228, 181);
    public static Color NavajoWhite => new(255, 222, 173);
    public static Color Navy => new(0, 0, 128);
    public static Color OldLace => new(253, 245, 230);
    public static Color Olive => new(128, 128, 0);
    public static Color OliveDrab => new(107, 142, 35);
    public static Color Orange => new(255, 165, 0);
    public static Color OrangeRed => new(255, 69, 0);
    public static Color Orchid => new(218, 112, 214);
    public static Color PaleGoldenrod => new(238, 232, 170);
    public static Color PaleGreen => new(152, 251, 152);
    public static Color PaleTurquoise => new(175, 238, 238);
    public static Color PaleVioletRed => new(219, 112, 147);
    public static Color PapayaWhip => new(255, 239, 213);
    public static Color PeachPuff => new(255, 218, 185);
    public static Color Peru => new(205, 133, 63);
    public static Color Pink => new(255, 192, 203);
    public static Color Plum => new(221, 160, 221);
    public static Color PowderBlue => new(176, 224, 230);
    public static Color Purple => new(128, 0, 128);
    public static Color Red => new(255, 0, 0);
    public static Color RosyBrown => new(188, 143, 143);
    public static Color RoyalBlue => new(65, 105, 225);
    public static Color SaddleBrown => new(139, 69, 19);
    public static Color Salmon => new(250, 128, 114);
    public static Color SandyBrown => new(244, 164, 96);
    public static Color SeaGreen => new(46, 139, 87);
    public static Color SeaShell => new(255, 245, 238);
    public static Color Sienna => new(160, 82, 45);
    public static Color Silver => new(192, 192, 192);
    public static Color SkyBlue => new(135, 206, 235);
    public static Color SlateBlue => new(106, 90, 205);
    public static Color SlateGray => new(112, 128, 144);
    public static Color Snow => new(255, 250, 250);
    public static Color SpringGreen => new(0, 255, 127);
    public static Color SteelBlue => new(70, 130, 180);
    public static Color Tan => new(210, 180, 140);
    public static Color Teal => new(0, 128, 128);
    public static Color Thistle => new(216, 191, 216);
    public static Color Tomato => new(255, 99, 71);
    public static Color Turquoise => new(64, 224, 208);
    public static Color Violet => new(238, 130, 238);
    public static Color Wheat => new(245, 222, 179);
    public static Color White => new(255, 255, 255);
    public static Color WhiteSmoke => new(245, 245, 245);
    public static Color Yellow => new(255, 255, 0);
    public static Color YellowGreen => new(154, 205, 50);

    public static Color FromHex(string hex)
    {
        hex = hex.Replace("#", string.Empty);

        var r = byte.Parse(hex[..2], NumberStyles.HexNumber);
        var g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        var b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

        return new Color(r, g, b);
    }

    public static Color FromHLS(double hue, double lighting, double saturation, long alpha = 255)
    {
        double r = 0, g = 0, b = 0;

        if (saturation == 0)
        {
            r = g = b = lighting; // achromatic
        }
        else
        {
            var q = lighting < 0.5 ? lighting * (1 + saturation) : lighting + saturation - lighting * saturation;
            var p = 2 * lighting - q;

            r = HueToRGB(p, q, hue + 1.0 / 3.0);
            g = HueToRGB(p, q, hue);
            b = HueToRGB(p, q, hue - 1.0 / 3.0);
        }

        return new Color((long)(r * 255), (long)(g * 255), (long)(b * 255), alpha);
    }

    private static double HueToRGB(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0 / 6.0) return p + (q - p) * 6 * t;
        if (t < 1.0 / 2.0) return q;
        if (t < 2.0 / 3.0) return p + (q - p) * (2.0 / 3.0 - t) * 6;

        return p;
    }

    public static Color Blend(Color color1, Color color2, double ratio)
    {
        if (ratio < 0 || ratio > 1)
            throw new ArgumentOutOfRangeException(nameof(ratio), "Ratio must be between 0 and 1");

        var r = (long)(color1.R * ratio + color2.R * (1 - ratio));
        var g = (long)(color1.G * ratio + color2.G * (1 - ratio));
        var b = (long)(color1.B * ratio + color2.B * (1 - ratio));
        var a = (long)(color1.A * ratio + color2.A * (1 - ratio))!;

        return new Color(r, g, b, a);
    }

    public readonly Color Invert()
    {
        return new Color(255 - R, 255 - G, 255 - B, A);
    }

    public static Color operator -(Color color)
    {
        return color.Invert();
    }

    public readonly Color ToGrayscale()
    {
        var gray = (long)(R * 0.3 + G * 0.59 + B * 0.11);
        return new Color(gray, gray, gray, A);
    }

    public readonly Color AdjustAlpha(long newAlpha)
    {
        return new Color(R, G, B, newAlpha);
    }

    public readonly Color AdjustBrightness(double factor)
    {
        var r = (long)(R * factor);
        var g = (long)(G * factor);
        var b = (long)(B * factor);

        r = Math.Clamp(r, 0, 255);
        g = Math.Clamp(g, 0, 255);
        b = Math.Clamp(b, 0, 255);

        return new Color(r, g, b, A);
    }
}