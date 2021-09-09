using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Osu.Music.Services.UItility
{
    public static class Extensions
    {
        public static string ToHex(this Color color) => color.ToString();

        public static Color FromHex(this string str) => (Color)ColorConverter.ConvertFromString(str);

        public static void SetMainColor(this Collection<ResourceDictionary> collection, string hex)
        {
            var dictionary = collection[0];

            Color mainColor = hex.FromHex();
            Color lightColor = new Color()
            {
                A = 255,
                R = (byte)Math.Min(Math.Floor(1.25 * mainColor.R), 255),
                G = (byte)Math.Min(Math.Floor(1.25 * mainColor.G), 255),
                B = (byte)Math.Min(Math.Floor(1.25 * mainColor.B), 255)
            };
            Color darkColor = new Color()
            {
                A = 255,
                R = (byte)Math.Floor(0.75 * mainColor.R),
                G = (byte)Math.Floor(0.75 * mainColor.G),
                B = (byte)Math.Floor(0.75 * mainColor.B)
            };

            dictionary["ColorMain"] = mainColor;
            dictionary["ColorMainLight"] = lightColor;
            dictionary["ColorMainDark"] = darkColor;

            dictionary["SolidColorBrushMain"] = new SolidColorBrush(mainColor);
            dictionary["SolidColorBrushMainLight"] = new SolidColorBrush(lightColor);
            dictionary["SolidColorBrushMainDark"] = new SolidColorBrush(darkColor);
        }
    }
}
