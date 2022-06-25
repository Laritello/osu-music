using System.Windows;
using System.Windows.Controls;

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
