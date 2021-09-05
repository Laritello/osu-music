using NAudio.Dsp;
using System;
using System.Linq;

namespace Osu.Music.Services.Audio
{
    /// <summary>
    /// Helps to analize Complex array from NAudio. Modified version of : https://stackoverflow.com/questions/55599743/naudio-fft-returns-small-and-equal-magnitude-values-for-all-frequencies
    /// </summary>
    public struct FrequencySpectrum
    {
        private readonly Complex[] frequencyDomain;
        public readonly int SamplingFrequency { get; }
        public readonly int FftWindowSize { get; }

        public FrequencySpectrum(Complex[] frequencyDomain, int samplingFrequency, int fftWindowSize)
        {
            if (frequencyDomain.Length == 0)
                throw new ArgumentException("Argument value must be greater than 0", nameof(frequencyDomain));
            if (samplingFrequency == 0)
                throw new ArgumentException("Argument value must be greater than 0", nameof(samplingFrequency));
            if (!((fftWindowSize & (fftWindowSize - 1)) == 0))
                throw new ArgumentException("Argument value must be power of two", nameof(fftWindowSize));

            this.frequencyDomain = frequencyDomain;

            SamplingFrequency = samplingFrequency;
            FftWindowSize = fftWindowSize;
        }

        /// <summary>
        /// Calculates magnitude for selected frequency.
        /// </summary>
        /// <param name="frequency">Selected frequency, Hz.</param>
        /// <returns>Magnitude for selected frequency.</returns>
        public float this[int frequency]
        {
            get
            {
                if (frequency < 0 || frequency >= SamplingFrequency)
                    throw new IndexOutOfRangeException();

                // Find corresponding bin
                float index = frequency / ((float)SamplingFrequency / FftWindowSize);
                Complex c = frequencyDomain[checked((uint)index)];
                return (float)Math.Sqrt((c.X * c.X) + (c.Y * c.Y));
            }
        }

        /// <summary>
        /// Calculates average magnitude for selected range of frequencies.
        /// </summary>
        /// <param name="startFrequency">Start of the frequency range, Hz.</param>
        /// <param name="endFrequency">End of the frequency range, Hz.</param>
        /// <returns>Average magnitude for selected range of frequencies.</returns>
        public float this[double startFrequency, double endFrequency]
        {
            get
            {
                if (startFrequency < 0 || startFrequency > SamplingFrequency)
                    throw new IndexOutOfRangeException(nameof(startFrequency));
                if (endFrequency < 0 || endFrequency > SamplingFrequency)
                    throw new IndexOutOfRangeException(nameof(endFrequency));

                // Find corresponding bins
                int startIndex = (int)Math.Round(startFrequency / ((float)SamplingFrequency / FftWindowSize));
                int endIndex = (int)Math.Round(endFrequency / ((float)SamplingFrequency / FftWindowSize));

                Complex[] range = frequencyDomain[startIndex..endIndex];
                return range.Select(c => (float)Math.Sqrt((c.X * c.X) + (c.Y * c.Y))).Average();
            }
        }
    }
}
