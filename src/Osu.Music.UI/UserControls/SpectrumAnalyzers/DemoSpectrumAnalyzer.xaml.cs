using NAudio.Dsp;
using Osu.Music.Services.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Osu.Music.UI.UserControls.SpectrumAnalyzers
{
    /// <summary>
    /// Логика взаимодействия для DemoSpectrumAnalyzer.xaml
    /// </summary>
    public partial class DemoSpectrumAnalyzer : UserControl
    {
        public int ColumnsCount { get; set; } = 32;

        public DemoSpectrumAnalyzer()
        {
            InitializeComponent();
        }

        public void Update(FrequencySpectrum spectrum)
        {
            float[] data = new float[ColumnsCount];
            float maximum = 0;
            double step = (double)spectrum.SamplingFrequency / 2 / ColumnsCount;

            for (double freq = 0; freq < spectrum.SamplingFrequency / 2; freq += step)
            {
                if (freq > spectrum.SamplingFrequency)
                    freq = spectrum.SamplingFrequency;

                int index = (int)(freq / step);
                data[index] = spectrum[freq, freq + step];

                if (data[index] > maximum)
                    maximum = data[index];
            }

            if (maximum != 0)
            {
                for (int i = 0; i < ColumnsCount; i++)
                    data[i] = data[i] / maximum;
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
            double height = value * ActualHeight;
            if (index >= canvas.Children.Count)
            {
                Rectangle rect = new Rectangle()
                {
                    Width = columnWidth,
                    Height = height,
                    Fill = Brushes.Purple
                };
                Canvas.SetLeft(rect, index * columnWidth);
                Canvas.SetBottom(rect, 0);

                canvas.Children.Add(rect);
            }
            else
            {
                Rectangle rect = (Rectangle)canvas.Children[index];
                rect.Width = columnWidth;
                rect.Height = Lerp(rect.Height, height, 0.8f);

                Canvas.SetLeft(rect, index * columnWidth);
                Canvas.SetBottom(rect, 0);
            }
        }

        private double Lerp(double firstFloat, double secondFloat, float by) => firstFloat * (1 - by) + secondFloat* by;
    }
}
