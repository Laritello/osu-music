using Osu.Music.UI.ViewModels;
using System.Windows;

namespace Osu.Music.UI.Utility
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

    #region Specific Implemantations
    public sealed class DialogCreatePlaylistProxy : BindingProxy<DialogCreatePlaylistViewModel> { }
    public sealed class DialogEditPlaylistProxy : BindingProxy<DialogEditPlaylistViewModel> { }
    #endregion
}
