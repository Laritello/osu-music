using Prism.Commands;
using System.ComponentModel;
using System.Windows;

namespace Osu.Music.UI.Utility
{
    public static class WindowClosingBehavior
    {
        public static readonly DependencyProperty ClosingProperty = DependencyProperty.RegisterAttached(
                "Closing",
                typeof(DelegateCommand),
                typeof(WindowClosingBehavior),
                new UIPropertyMetadata(new PropertyChangedCallback(ClosingChanged)));

        public static DelegateCommand GetClosing(DependencyObject obj)
        {
            return (DelegateCommand)obj.GetValue(ClosingProperty);
        }

        public static void SetClosing(DependencyObject obj, DelegateCommand value)
        {
            obj.SetValue(ClosingProperty, value);
        }

        private static void ClosingChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target == null || !(target is Window window))
                return;

            if (window != null)
            {
                if (e.NewValue != null)
                    window.Closing += Window_Closing;
                else
                    window.Closing -= Window_Closing;
            }
        }

        private static void Window_Closing(object sender, CancelEventArgs e)
        {
            var window = sender as Window;

            if (window != null)
            {
                var closing = GetClosing(window);

                if (closing != null)
                {
                    if (closing.CanExecute())
                        closing.Execute();
                    else
                        e.Cancel = true;
                }
            }
        }
    }
}
