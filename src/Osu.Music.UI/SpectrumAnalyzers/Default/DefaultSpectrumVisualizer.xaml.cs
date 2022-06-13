using Osu.Music.Services.Audio;
using Osu.Music.UI.SpectrumAnalyzers.Default;
using System.Windows.Controls;

namespace Osu.Music.UI.SpectrumAnalyzers
{
    /// <summary>
    /// Логика взаимодействия для DefaultSpectrumVisualizer.xaml
    /// </summary>
    public partial class DefaultSpectrumVisualizer : UserControl
    {
        public int ColumnsCount { get; set; } = 18;

        public DefaultSpectrumVisualizer()
        {
            InitializeComponent();
            InitializeBars();
        }

        private void InitializeBars()
        {
            for (int i = 0; i < ColumnsCount; i++)
            {
                Bars.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star)
                });

                var bar = new SpectrumBar();
                Grid.SetColumn(bar, i);

                Bars.Children.Add(bar);
            }
        }

        public void Update(FrequencySpectrum spectrum)
        {
            float[] data = spectrum.GetLogarithmicallyScaledSpectrum();
            UpdateBars(data);
        }

        private void UpdateBars(float[] data)
        {
            for (var i = 0; i < ColumnsCount; i++)
            {
                var bar = Bars.Children[i] as SpectrumBar;
                bar.Value = data[i];
            }
        }
    }
}
