using NAudio.Dsp;
using Osu.Music.Services.Audio;
using System;
using System.Diagnostics;

namespace Osu.Music.Services.Events
{
    public class FftEventArgs : EventArgs
    {
        [DebuggerStepThrough]
        public FftEventArgs(Complex[] result, int samplingFrequency, int fftWindowSize) => Result = new FrequencySpectrum(result, samplingFrequency, fftWindowSize);
        public FrequencySpectrum Result { get; private set; }
    }
}
