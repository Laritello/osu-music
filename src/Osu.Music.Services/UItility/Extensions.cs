using MaterialDesignThemes.Wpf;
using Osu.Music.Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static void UpdateColorScheme(this Collection<ResourceDictionary> collection, string hex, ApplicationTheme theme)
        {
            var regex = new Regex("Colors.xaml");
            var dictionary = collection.FirstOrDefault(x => regex.IsMatch(string.IsNullOrEmpty(x.Source?.OriginalString) ? string.Empty : x.Source?.OriginalString));

            var colors = CreateColors(hex, theme);

            dictionary["ColorMain"] = colors["ColorMain"];
            dictionary["ColorMainLight"] = colors["ColorMainLight"];
            dictionary["ColorMainDark"] = colors["ColorMainDark"];

            dictionary["ColorHover"] = colors["ColorHover"];
            dictionary["ColorBackground"] = colors["ColorBackground"];
            dictionary["ColorForeground"] = colors["ColorForeground"];
            dictionary["ColorBorder"] = colors["ColorBorder"];

            dictionary["ColorTitleBarHover"] = colors["ColorTitleBarHover"];
            dictionary["ColorTitleBarPressed"] = colors["ColorTitleBarPressed"];

            dictionary["ColorTextDisabled"] = colors["ColorTextDisabled"];
            dictionary["ColorTextMediumEmphasis"] = colors["ColorTextMediumEmphasis"];
            dictionary["ColorTextHighEmphasis"] = colors["ColorTextHighEmphasis"];

            dictionary["SolidColorBrushMain"] = new SolidColorBrush(colors["ColorMain"]);
            dictionary["SolidColorBrushMainLight"] = new SolidColorBrush(colors["ColorMainLight"]);
            dictionary["SolidColorBrushMainDark"] = new SolidColorBrush(colors["ColorMainDark"]);

            dictionary["SolidColorBrushHover"] = new SolidColorBrush(colors["ColorHover"]);
            dictionary["SolidColorBrushBackground"] = new SolidColorBrush(colors["ColorBackground"]);
            dictionary["SolidColorBrushForeground"] = new SolidColorBrush(colors["ColorForeground"]);
            dictionary["SolidColorBrushBorder"] = new SolidColorBrush(colors["ColorBorder"]);

            dictionary["SolidColorBrushTitleBarHover"] = new SolidColorBrush(colors["ColorTitleBarHover"]);
            dictionary["SolidColorBrushTitleBarPressed"] = new SolidColorBrush(colors["ColorTitleBarPressed"]);

            dictionary["SolidColorBrushTextDisabled"] = new SolidColorBrush(colors["ColorTextDisabled"]);
            dictionary["SolidColorBrushTextMediumEmphasis"] = new SolidColorBrush(colors["ColorTextMediumEmphasis"]);
            dictionary["SolidColorBrushTextHighEmphasis"] = new SolidColorBrush(colors["ColorTextHighEmphasis"]);

            CustomColorTheme material = collection[1] as CustomColorTheme;
            material.PrimaryColor = colors["ColorMain"];
            material.BaseTheme = theme == ApplicationTheme.Light ? BaseTheme.Light : BaseTheme.Dark;
        }

        private static Dictionary<string, Color> CreateColors(string hex, ApplicationTheme theme)
        {
            var dictionary = new Dictionary<string, Color>();

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

            switch (theme)
            {
                case ApplicationTheme.Light:
                    dictionary["ColorHover"] = "#10000000".FromHex();
                    dictionary["ColorBackground"] = "#FFFFFFFF".FromHex();
                    dictionary["ColorForeground"] = "#FF000000".FromHex();
                    dictionary["ColorBorder"] = "#20000000".FromHex();

                    dictionary["ColorTitleBarHover"] = "#19000000".FromHex();
                    dictionary["ColorTitleBarPressed"] = "#35000000".FromHex();

                    dictionary["ColorTextDisabled"] = "#61000000".FromHex();
                    dictionary["ColorTextMediumEmphasis"] = "#99000000".FromHex();
                    dictionary["ColorTextHighEmphasis"] = "#DE000000".FromHex();
                    break;

                case ApplicationTheme.Dark:
                    dictionary["ColorHover"] = "#10FFFFFF".FromHex();
                    dictionary["ColorBackground"] = "#FF121212".FromHex();
                    dictionary["ColorForeground"] = "#FFFFFFFF".FromHex();
                    dictionary["ColorBorder"] = "#20FFFFFF".FromHex();

                    dictionary["ColorTitleBarHover"] = "#19FFFFFF".FromHex();
                    dictionary["ColorTitleBarPressed"] = "#35FFFFFF".FromHex();

                    dictionary["ColorTextDisabled"] = "#61FFFFFF".FromHex();
                    dictionary["ColorTextMediumEmphasis"] = "#99FFFFFF".FromHex();
                    dictionary["ColorTextHighEmphasis"] = "#DEFFFFFF".FromHex(); 
                    break;
            }

            return dictionary;
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

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static ResourceDictionary GetLocalizationDictionary(this ResourceDictionary dictionary)
        {
            var regex = new Regex("osu.Music.([a-z]{2})-([A-Z]{2}).xaml");
            return dictionary.MergedDictionaries.FirstOrDefault(x => regex.IsMatch(string.IsNullOrEmpty(x.Source?.OriginalString) ? string.Empty : x.Source?.OriginalString));
        }
    }
}
