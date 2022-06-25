using System.Windows;

namespace Osu.Music.UI.Utility.Proxies
{
    #region Base Class
    public class BindingProxy<T> : Freezable
    {
        #region Overrides of Freezable
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy<T>();
        }
        #endregion

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy<T>), new UIPropertyMetadata(null));
        public T Data
        {
            get => (T)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
    #endregion
}
