using Osu.Music.Services.Audio;

namespace Osu.Music.UI.Interfaces
{
    public interface IVisualizationPlugin
    {
        object Content { get; }
        void OnFftCalculated(FrequencySpectrum result);
    }
}
