using Osu.Music.Services.Audio;
using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Osu.Music.UI.UserControls.SpectrumAnalyzers
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
        }

        public void Update(FrequencySpectrum spectrum)
        {
            float[] data = spectrum.GetLogarithmicallyScaledSpectrum();
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
            double height = value < 0 ? 0 : value * canvas.ActualHeight;
            if (index >= canvas.Children.Count)
            {
                Rectangle rect = new Rectangle()
                {
                    Width = Math.Ceiling(columnWidth),
                    Height = height,
                };

                rect.SetResourceReference(Shape.FillProperty, "SolidColorBrushMain");

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

        private double Lerp(double firstFloat, double secondFloat, float by) => (firstFloat * (1 - by)) + (secondFloat * by);
    }
}
