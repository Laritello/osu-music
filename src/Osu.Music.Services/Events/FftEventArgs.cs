using NAudio.Dsp;
using Osu.Music.Services.Audio;
using System;
using System.Diagnostics;

namespace Osu.Music.Services.Events
{
	[method: DebuggerStepThrough]
	public class FftEventArgs(Complex[] result, int samplingFrequency, int fftWindowSize) : EventArgs
	{
		public FrequencySpectrum Result { get; } = new FrequencySpectrum(result, samplingFrequency, fftWindowSize);
	}
}
