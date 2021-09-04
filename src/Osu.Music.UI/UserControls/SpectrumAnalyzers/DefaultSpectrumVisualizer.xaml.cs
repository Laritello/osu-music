using Osu.Music.Services.Audio;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Osu.Music.UI.UserControls.SpectrumAnalyzers
{
    /// <summary>
    /// Логика взаимодействия для DefaultSpectrumVisualizer.xaml
    /// </summary>
    public partial class DefaultSpectrumVisualizer : UserControl
    {
        public int ColumnsCount { get; set; } = 18;

        public double MinimumFrequency { get; set; } = 20;
        public double MaximumFrequency { get; set; } = 20000;

        public DefaultSpectrumVisualizer()
        {
            InitializeComponent();
        }

        public void Update(FrequencySpectrum spectrum)
        {
            float[] data = new float[ColumnsCount];

            double minFreq = Math.Max(0, MinimumFrequency);
            double maxFreq = Math.Min(spectrum.SamplingFrequency, MaximumFrequency);
            double step = (double)maxFreq / ColumnsCount;



            for (double freq = minFreq; freq < maxFreq; freq += step)
            {
                if (freq > spectrum.SamplingFrequency)
                    freq = spectrum.SamplingFrequency;

                int index = (int)(freq / step);
                data[index] = (float)(10 * Math.Log10(spectrum[freq, freq + step])); // Probably correct visualization
            }

            float minimum = data.Min();

            if (minimum != 0)
            {
                for (int i = 0; i < ColumnsCount; i++)
                    data[i] = 1 - data[i] / minimum;
            }

            UpdateCanvas(data);
        }

        private void UpdateCanvas(float[] data)
        {
            double columnWidth = ActualWidth / ColumnsCount;

            for (int col = 0; col < ColumnsCount; col++)
                AddResult(col, columnWidth, data[col]);
        }

        private void AddResult(int index, double columnWidth, float value)
        {
            double height = double.IsNaN(value * ActualHeight) ? 0 : value * ActualHeight;
            if (index >= canvas.Children.Count)
            {
                Rectangle rect = new Rectangle()
                {
                    Width = Math.Ceiling(columnWidth),
                    Height = height,
                    Fill = Brushes.Purple
                };
                Canvas.SetLeft(rect, Math.Floor(index * columnWidth));
                Canvas.SetBottom(rect, 0);

                canvas.Children.Add(rect);
            }
            else
            {
                Rectangle rect = (Rectangle)canvas.Children[index];
                rect.Width = Math.Ceiling(columnWidth);
                rect.Height = Lerp(rect.Height, height, 0.8f);

                Canvas.SetLeft(rect, Math.Floor(index * columnWidth));
                Canvas.SetBottom(rect, 0);
            }
        }

        private double Lerp(double firstFloat, double secondFloat, float by) => firstFloat * (1 - by) + secondFloat* by;
    }
}
