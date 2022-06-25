using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Osu.Music.Services.UItility
{
    public static class Extensions
    {
        public static string ToHex(this Color color) => color.ToString();

        public static Color FromHex(this string str) => (Color)ColorConverter.ConvertFromString(str);

        public static Color GetMainColor(this Collection<ResourceDictionary> collection)
        {
            return collection[0]["ColorMain"] != null ? (Color)collection[0]["ColorMain"] : Colors.Transparent;
        }

        public static void SetMainColor(this Collection<ResourceDictionary> collection, string hex)
        {
            // Using index is cool, but if I change order of dictionaries in App.xaml
            // it won't be that cool anymore. Maybe switch to naming or someting.
            ResourceDictionary dictionary = collection[0];

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
            Color borderColor = new Color()
            {
                A = 64,
                R = mainColor.R,
                G = mainColor.G,
                B = mainColor.B
            };

            dictionary["ColorMain"] = mainColor;
            dictionary["ColorMainLight"] = lightColor;
            dictionary["ColorMainDark"] = darkColor;
            dictionary["ColorMainBorder"] = borderColor;

            dictionary["SolidColorBrushMain"] = new SolidColorBrush(mainColor);
            dictionary["SolidColorBrushMainLight"] = new SolidColorBrush(lightColor);
            dictionary["SolidColorBrushMainDark"] = new SolidColorBrush(darkColor);
            dictionary["SolidColorBrushMainBorder"] = new SolidColorBrush(borderColor);

            CustomColorTheme theme = collection[1] as CustomColorTheme;
            theme.PrimaryColor = mainColor;
        }

        public static ulong ToUnix(this DateTime dt)
        {
            return (ulong)dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }

        public static double Median(this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count() == 0) throw new ArgumentException("Sequence has no elements");

            var sorted = source.OrderBy(x => x).ToArray();
            int size = sorted.Length;

            return size % 2 != 0 ?
                sorted[(int)Math.Ceiling(size / 2.0) - 1] :
                (sorted[(size / 2) - 1] + sorted[size / 2]) / 2;
        }

        public static double Median(this IEnumerable<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count() == 0) throw new ArgumentException("Sequence has no elements");

            var sorted = source.OrderBy(x => x).ToArray();
            int size = sorted.Length;

            return size % 2 != 0 ?
                sorted[(int)Math.Ceiling(size / 2.0) - 1] :
                (sorted[(size / 2) - 1] + sorted[size / 2]) / 2;
        }

        public static float Median(this IEnumerable<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count() == 0) throw new ArgumentException("Sequence has no elements");

            var sorted = source.OrderBy(x => x).ToArray();
            int size = sorted.Length;

            return size % 2 != 0 ?
                sorted[(int)Math.Ceiling(size / 2.0) - 1] :
                (sorted[(size / 2) - 1] + sorted[size / 2]) / 2;
        }

        public static bool ContainsLower(this string source, string value) => source.ToLower().Contains(value.ToLower());
    }
}
