using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Osu.Music.UI.Behaviors
{
    public static class ValidationBehavior
    {
        #region Attached Properties
        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.RegisterAttached(
            "HasError",
            typeof(bool),
            typeof(ValidationBehavior),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceHasError));

        private static readonly DependencyProperty HasErrorDescriptorProperty = DependencyProperty.RegisterAttached(
            "HasErrorDescriptor",
            typeof(DependencyPropertyDescriptor),
            typeof(ValidationBehavior));
        #endregion

        private static DependencyPropertyDescriptor GetHasErrorDescriptor(DependencyObject d)
        {
            return (DependencyPropertyDescriptor)d.GetValue(HasErrorDescriptorProperty);
        }

        private static void SetHasErrorDescriptor(DependencyObject d, DependencyPropertyDescriptor value)
        {
            d.SetValue(HasErrorDescriptorProperty, value);
        }

        #region Attached Property Getters and setters
        public static bool GetHasError(DependencyObject d)
        {
            return (bool)d.GetValue(HasErrorProperty);
        }

        public static void SetHasError(DependencyObject d, bool value)
        {
            d.SetValue(HasErrorProperty, value);
        }
        #endregion

        #region CallBacks
        private static object CoerceHasError(DependencyObject d, object baseValue)
        {
            var result = (bool)baseValue;
            if (BindingOperations.IsDataBound(d, HasErrorProperty))
            {
                if (GetHasErrorDescriptor(d) == null)
                {
                    var desc = DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.Validation.HasErrorProperty, d.GetType());
                    desc.AddValueChanged(d, OnHasErrorChanged);
                    SetHasErrorDescriptor(d, desc);
                    result = System.Windows.Controls.Validation.GetHasError(d);
                }
            }
            else
            {
                if (GetHasErrorDescriptor(d) != null)
                {
                    var desc = GetHasErrorDescriptor(d);
                    desc.RemoveValueChanged(d, OnHasErrorChanged);
                    SetHasErrorDescriptor(d, null);
                }
            }
            return result;
        }
        private static void OnHasErrorChanged(object sender, EventArgs e)
        {
            if (sender is DependencyObject d)
            {
                d.SetValue(HasErrorProperty, d.GetValue(System.Windows.Controls.Validation.HasErrorProperty));
            }
        }
        #endregion
    }
}
