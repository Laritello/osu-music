using Osu.Music.Services.Audio;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.SpectrumAnalyzers;

namespace Osu.Music.UI.Visualization
{
    class DefaultVisualization : IVisualizationPlugin
    {
        private readonly DefaultSpectrumVisualizer visualizer = new DefaultSpectrumVisualizer();

        public object Content => visualizer;

        public void OnFftCalculated(FrequencySpectrum result)
        {
            visualizer.Update(result);
        }
    }
}
