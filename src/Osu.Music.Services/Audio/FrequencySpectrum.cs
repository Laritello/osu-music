using NAudio.Dsp;
using System;
using System.Linq;

namespace Osu.Music.Services.Audio
{
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


        //returns magnitude for freq
        public float this[int freq]
        {
            get
            {
                if (freq < 0 || freq >= SamplingFrequency)
                    throw new IndexOutOfRangeException();

                // Find corresponding bin
                float index = freq / ((float)SamplingFrequency / FftWindowSize);
                Complex c = frequencyDomain[checked((uint)index)];
                return (float)Math.Sqrt((c.X * c.X) + (c.Y * c.Y));
            }
        }

        public float this[double startFreq, double endFreq]
        {
            get
            {
                
                if (startFreq < 0 || startFreq > SamplingFrequency)
                    throw new IndexOutOfRangeException(nameof(startFreq));
                if (endFreq < 0 || endFreq > SamplingFrequency)
                    throw new IndexOutOfRangeException(nameof(endFreq));

                // Find corresponding bin
                int startIndex = (int)Math.Round(startFreq / ((float)SamplingFrequency / FftWindowSize));
                int endIndex = (int)Math.Round(endFreq / ((float)SamplingFrequency / FftWindowSize));

                Complex[] range = frequencyDomain[startIndex..endIndex];
                return range.Select(c => (float)Math.Sqrt((c.X * c.X) + (c.Y * c.Y))).Average();
            }
        }
    }
}
