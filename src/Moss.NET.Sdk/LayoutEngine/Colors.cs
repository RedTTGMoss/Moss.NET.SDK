namespace Moss.NET.Sdk.LayoutEngine;

public static class Colors
{
    public static readonly Color Red = new(255, 0, 0);
    public static readonly Color Green = new(0, 255, 0);
    public static readonly Color Blue = new(0, 0, 255);
    public static readonly Color Yellow = new(255, 255, 0);
    public static readonly Color Cyan = new(0, 255, 255);
    public static readonly Color Magenta = new(255, 0, 255);
    public static readonly Color Silver = new(192, 192, 192);
    public static readonly Color Gray = new(128, 128, 128);
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(255, 255, 255);
    public static readonly Color DarkRed = new(128, 0, 0);
    public static readonly Color Olive = new(128, 128, 0);
    public static readonly Color DarkGreen = new(0, 128, 0);
    public static readonly Color Purple = new(128, 0, 128);
    public static readonly Color Teal = new(0, 128, 128);
    public static readonly Color Navy = new(0, 0, 128);
    public static readonly Color Orange = new(255, 165, 0);
    public static readonly Color Pink = new(255, 192, 203);
    public static readonly Color Gold = new(255, 215, 0);
    public static readonly Color LightBlue = new(173, 216, 230);
    public static readonly Color Indigo = new(75, 0, 130);
    public static readonly Color Khaki = new(240, 230, 140);
    public static readonly Color MediumSeaGreen = new(60, 179, 113);
    public static readonly Color MediumPurple = new(123, 104, 238);
    public static readonly Color SandyBrown = new(244, 164, 96);
    public static readonly Color Creme = new Color(244, 244, 244);
    public static readonly Color LightGray = new(211, 211, 211);
    public static readonly Color DarkGray = new(169, 169, 169);
    public static readonly Color LightCoral = new(240, 128, 128);
    public static readonly Color LightGreen = new(144, 238, 144);
    public static readonly Color LightPink = new(255, 182, 193);
    public static readonly Color LightSalmon = new(255, 160, 122);
    public static readonly Color WhiteSmoke = new(245, 245, 245);

    public static Color Parse(string color)
    {
        if (color.StartsWith('#'))
        {
            return FromHex(color);
        }

        return FromName(color);
    }

    public static Color FromHex(string hex)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        if (hex.Length != 6 && hex.Length != 8)
        {
            throw new ArgumentException("Hex-Farbe muss 6 oder 8 Zeichen lang sein.");
        }

        byte r = Convert.ToByte(hex.Substring(0, 2), 16);
        byte g = Convert.ToByte(hex.Substring(2, 2), 16);
        byte b = Convert.ToByte(hex.Substring(4, 2), 16);
        byte a = hex.Length == 8 ? Convert.ToByte(hex.Substring(6, 2), 16) : (byte)255;

        return new Color(r, g, b);
    }

    public static Color FromName(string name)
    {
        return name.ToLower() switch
        {
            "red" => Red,
            "green" => Green,
            "blue" => Blue,
            "yellow" => Yellow,
            "cyan" => Cyan,
            "magenta" => Magenta,
            "silver" => Silver,
            "gray" => Gray,
            "black" => Black,
            "white" => White,
            "darkred" => DarkRed,
            "olive" => Olive,
            "darkgreen" => DarkGreen,
            "purple" => Purple,
            "teal" => Teal,
            "navy" => Navy,
            "orange" => Orange,
            "pink" => Pink,
            "gold" => Gold,
            "lightblue" => LightBlue,
            "indigo" => Indigo,
            "khaki" => Khaki,
            "mediumseagreen" => MediumSeaGreen,
            "mediumpurple" => MediumPurple,
            "sandybrown" => SandyBrown,
            "creme" => Creme,
            "lightgray" => LightGray,
            "darkgray" => DarkGray,
            "lightcoral" => LightCoral,
            "lightgreen" => LightGreen,
            "lightpink" => LightPink,
            "lightsalmon" => LightSalmon,
            "whitesmoke" => WhiteSmoke,
            _ => throw new ArgumentException($"Color '{name}' not found.")
        };
    }
}