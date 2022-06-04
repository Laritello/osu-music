using Osu.Music.Services.Audio;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.SpectrumAnalyzers;

namespace Osu.Music.UI.Visualization
{
    class CircleVisualization : IVisualizationPlugin
    {
        private readonly CircleSpectrumVisualizer visualizer = new CircleSpectrumVisualizer();

        public object Content => visualizer;

        public void OnFftCalculated(FrequencySpectrum result)
        {
            visualizer.Update(result);
        }
    }
}
