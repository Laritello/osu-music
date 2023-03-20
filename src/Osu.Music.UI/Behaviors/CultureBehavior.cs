using Microsoft.Xaml.Behaviors;
using Osu.Music.Services.Localization;
using System.Windows;
using System.Windows.Controls;

namespace Osu.Music.UI.Behaviors
{
    public class CultureBehavior : Behavior<FrameworkElement>
    {
        private FrameworkElement _HostingControl;
        private DependencyProperty _HostingControlDependencyProperty;

        protected override void OnAttached()
        {
            base.OnAttached();

            _HostingControl = AssociatedObject;

            InitHostingControl();
            LocalizationManager.Instance.CultureChanged += LocalizationManager_CultureChanged;
        }

        protected override void OnDetaching()
        {
            LocalizationManager.Instance.CultureChanged -= LocalizationManager_CultureChanged;
            base.OnDetaching();
        }

        private void LocalizationManager_CultureChanged(LocalizationCulture culture)
        {
            if (_HostingControlDependencyProperty != null)
                _HostingControl.GetBindingExpression(_HostingControlDependencyProperty).UpdateTarget();
        }

        private void InitHostingControl()
        {
            if (_HostingControl is TextBlock)
                _HostingControlDependencyProperty = TextBlock.TextProperty;
            else if (_HostingControl is TextBox)
                _HostingControlDependencyProperty = TextBox.TextProperty;
        }
    }
}
