using System.Windows.Media;

namespace Osu.Music.Services.UItility
{
    public static class Extensions
    {
        public static string ToHex(this Color color) => color.ToString();

        public static Color FromHex(this string str) => (Color)ColorConverter.ConvertFromString(str);
    }
}
