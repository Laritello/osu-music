using Prism.Mvvm;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class SelectedPageToToggleCheckCovnerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            if (!(value is BindableBase))
                throw new ArgumentException("Input argument must be of type BindableBase");

            var type = value.GetType().Name;
            var name = parameter as string;
            return type == name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
