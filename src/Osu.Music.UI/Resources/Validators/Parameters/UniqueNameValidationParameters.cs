using System.Collections.Generic;
using System.Windows;

namespace Osu.Music.UI.Resources.Validators
{
    public class UniqueNameValidationParameters : DependencyObject
    {
        public static readonly DependencyProperty NamesProperty = DependencyProperty.Register("Names", typeof(IEnumerable<string>), typeof(UniqueNameValidationParameters), new PropertyMetadata(null));
        public IEnumerable<string> Names
        {
            get => (IEnumerable<string>)GetValue(NamesProperty);
            set => SetValue(NamesProperty, value);
        }
    }
}
