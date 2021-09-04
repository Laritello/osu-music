using NAudio.Dsp;
using Osu.Music.Services.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osu.Music.UI.Interfaces
{
    public interface IVisualizationPlugin
    {
        object Content { get; }
        void OnFftCalculated(FrequencySpectrum result);
    }
}
