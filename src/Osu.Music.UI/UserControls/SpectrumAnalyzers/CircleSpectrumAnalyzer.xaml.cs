using NAudio.Dsp;
using Osu.Music.Services.Audio;
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

        public CircleSpectrumAnalyzer()
        {
            InitializeComponent();
        }

        public void Update(FrequencySpectrum fftResults)
        {

        }


        private void AddResult(int index, double angleStep, double relativeValue)
        {

        }
    }
}
