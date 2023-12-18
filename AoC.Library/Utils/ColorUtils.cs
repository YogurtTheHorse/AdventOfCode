using System.Drawing;
using System.Globalization;

namespace AoC.Library.Utils;

public static class ColorUtils
{
    public static Color ParseHex(string hex)
    {
        hex = hex.TrimStart('#');

        return hex.Length == 8
            ? Color.FromArgb(
                int.Parse(hex[..2], NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber),
                int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber))
            : Color.FromArgb(255, // hardcoded opaque
                int.Parse(hex[..2], NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber));
    }
}