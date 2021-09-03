using NAudio.Dsp;
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
        private double xScale = 200;
        private int bins = 128; // guess a 1024 size FFT, bins is half FFT size

        public DemoSpectrumAnalyzer()
        {
            InitializeComponent();
        }

        private const int binsPerPoint = 1; // reduce the number of points we plot for a less jagged line?
        private int updateCount;

        public void Update(Complex[] fftResults)
        {
            // no need to repaint too many frames per second
            if (updateCount++ % 2 == 0)
            {
                return;
            }

            if (fftResults.Length != bins)
            {
                bins = fftResults.Length;
            }

            List<double> dBs = new List<double>();

            for (int n = 0; n < fftResults.Length; n += binsPerPoint)
            {
                // averaging out bins
                double yPos = 0;
                for (int b = 0; b < binsPerPoint; b++)
                    yPos += GetYPosLog(fftResults[n + b]);

                dBs.Add(yPos / binsPerPoint);
            }

            double max = dBs.Select(x => Math.Abs(x)).Max();

            for (int i = 0; i < dBs.Count; i++)
                dBs[i] = Math.Abs(dBs[i] / max) * ActualHeight;

            for (int i = 0; i < dBs.Count; i++)
                AddResult(i, dBs[i]);
        }

        private double GetYPosLog(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            double intensityDB = 10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -90;
            if (intensityDB < minDB) intensityDB = minDB;
            double percent = intensityDB / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            double yPos = percent * ActualHeight;
            //return yPos;
            return intensityDB;
        }

        private void AddResult(int index, double power)
        {
            if (index >= canvas.Children.Count)
            {
                Line l = new Line()
                {
                    X1 = index,
                    Y1 = ActualHeight,
                    X2 = index,
                    Y2 = power,
                    Stroke = new SolidColorBrush(Color.FromRgb(128, 0, 128))
                };
                canvas.Children.Add(l);
            }
            else
            {
                Line l = (Line)canvas.Children[index];
                l.X1 = index;
                l.Y1 = ActualHeight;
                l.X2 = index;
                l.Y2 = Lerp(l.Y2, power, 0.8f);
            }
        }

        private double Lerp(double firstFloat, double secondFloat, float by) => firstFloat * (1 - by) + secondFloat* by;
    }
}
