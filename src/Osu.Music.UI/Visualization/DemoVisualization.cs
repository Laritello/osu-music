using Osu.Music.Services.Audio;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.UserControls.SpectrumAnalyzers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osu.Music.UI.Visualization
{
    class DemoVisualization : IVisualizationPlugin
    {
        private readonly DemoSpectrumAnalyzer spectrumAnalyser = new DemoSpectrumAnalyzer();

        public object Content => spectrumAnalyser;

        public void OnMaxCalculated(float min, float max)
        {
            // nothing to do
        }

        public void OnFftCalculated(FrequencySpectrum result)
        {
            spectrumAnalyser.Update(result);
        }
    }
}
