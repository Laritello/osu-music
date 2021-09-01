using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osu.Music.UI.Interfaces
{
    public interface IVisualizationPlugin
    {
        object Content { get; }

        // n.b. not great design, need to refactor so visualizations can attach to the playback graph and measure just what they need
        void OnMaxCalculated(float min, float max);
        void OnFftCalculated(Complex[] result);
    }
}
