using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Osu.Music.UI.SpectrumAnalyzers.Default
{
    /// <summary>
    /// Логика взаимодействия для SpectrumBar.xaml
    /// </summary>
    public partial class SpectrumBar : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(SpectrumBar), new PropertyMetadata(0.0));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public SpectrumBar()
        {
            InitializeComponent();
        }
    }
}
