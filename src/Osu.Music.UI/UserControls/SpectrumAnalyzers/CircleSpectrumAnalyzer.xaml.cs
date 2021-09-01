using NAudio.Dsp;
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

namespace Osu.Music.UI.UserControls.SpectrumAnalyzers
{
    /// <summary>
    /// Логика взаимодействия для CircleSpectrumAnalyzer.xaml
    /// </summary>
    public partial class CircleSpectrumAnalyzer : UserControl
    {
        private int bins = 512; // guess a 1024 size FFT, bins is half FFT size

        private int circleRadius = 65;
        private int padding = 10;

        public CircleSpectrumAnalyzer()
        {
            InitializeComponent();
        }

        private const int binsPerPoint = 2; // reduce the number of points we plot for a less jagged line?
        private int updateCount;

        public void Update(Complex[] fftResults)
        {
            // no need to repaint too many frames per second
            if (updateCount++ % 2 == 0)
                return;

            if (fftResults.Length / 2 != bins)
                bins = fftResults.Length / 2;

            int totalPoints = fftResults.Length / 2 / binsPerPoint;
            double angleStep = 2 * Math.PI / totalPoints;
            

            for (int n = 0; n < fftResults.Length / 2; n += binsPerPoint)
                AddResult(n / binsPerPoint, angleStep, GetAverageNormalizedValue(fftResults, n));
            

            AddResult(totalPoints, 0, GetAverageNormalizedValue(fftResults, 0));
        }

        private double GetAverageNormalizedValue(Complex[] fftResults, int startingIndex)
        {
            double relativeValue = 0;
            for (int b = 0; b < binsPerPoint; b++)
                relativeValue += GetRelativeValue(fftResults[startingIndex + b]);

            return relativeValue /= binsPerPoint;
        }

        private double GetRelativeValue(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            double intensityDB = 10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -90;
            if (intensityDB < minDB) intensityDB = minDB;
            double percent = intensityDB / minDB;

            // we want 0dB to be at the top (i.e. percent = 1)
            return 1 - percent;
        }

        private void AddResult(int index, double angleStep, double relativeValue)
        {
            double halfWidth = ActualWidth / 2;
            double halfHeight = ActualHeight / 2;

            int x = (int)Math.Floor(halfWidth + (Math.Cos(angleStep * index) * ((halfWidth - padding - circleRadius) * relativeValue + circleRadius)));
            int y = (int)Math.Floor(halfHeight + (Math.Sin(angleStep * index) * ((halfHeight - padding - circleRadius) * relativeValue + circleRadius)));

            Point p = new Point(x, y);
            if (index >= polyline1.Points.Count)
                polyline1.Points.Add(p);
            else
                polyline1.Points[index] = p;
        }
    }
}
