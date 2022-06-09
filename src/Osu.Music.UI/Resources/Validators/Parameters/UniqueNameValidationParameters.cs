using System.Collections.Generic;
using System.Windows;

namespace Osu.Music.UI.Resources.Validators
{
    public class UniqueNameValidationParameters : DependencyObject
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<object>), typeof(UniqueNameValidationParameters), new PropertyMetadata(null));
        public IEnumerable<object> Items
        {
            get => (IEnumerable<object>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(object), typeof(UniqueNameValidationParameters), new PropertyMetadata(null));
        public object Item
        {
            get => GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
    }
}
