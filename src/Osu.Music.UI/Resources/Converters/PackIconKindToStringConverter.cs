using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    internal class PackIconKindToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertToName((PackIconKind)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string ConvertToName(PackIconKind icon)
        {
            var text = icon.ToString();

            if (string.IsNullOrWhiteSpace(text))
                return "";

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}
