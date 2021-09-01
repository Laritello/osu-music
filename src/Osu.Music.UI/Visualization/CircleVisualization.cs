using Osu.Music.UI.Interfaces;
using Osu.Music.UI.UserControls.SpectrumAnalyzers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osu.Music.UI.Visualization
{
    class CircleVisualization : IVisualizationPlugin
    {
        private readonly CircleSpectrumAnalyzer spectrumAnalyser = new CircleSpectrumAnalyzer();

        public object Content => spectrumAnalyser;

        public void OnMaxCalculated(float min, float max)
        {
            // nothing to do
        }

        public void OnFftCalculated(NAudio.Dsp.Complex[] result)
        {
            spectrumAnalyser.Update(result);
        }
    }
}
