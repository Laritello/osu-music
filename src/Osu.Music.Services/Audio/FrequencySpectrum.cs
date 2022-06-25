using NAudio.Dsp;
using Osu.Music.Services.UItility;
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

                if (startIndex == endIndex)
                {
                    Complex c = frequencyDomain[checked((uint)startIndex)];
                    return (float)Math.Sqrt((c.X * c.X) + (c.Y * c.Y));
                }
                else
                {
                    Complex[] range = frequencyDomain[startIndex..endIndex];
                    return range.Length > 0 ? range.Select(c => (float)Math.Sqrt((c.X * c.X) + (c.Y * c.Y))).Median() : 0;
                }
            }
        }

        public float[] GetLogarithmicallyScaledSpectrum(
            int minFrequency = 10,
            int maxFrequency = 44100,
            int binsCount = 18,
            float maxDb = -30,
            float minDb = -90,
            SpectrumSize size = SpectrumSize.Half)
        {
            if (minFrequency < 0)
                throw new ArgumentException(nameof(minFrequency));

            if (binsCount == 0)
                throw new ArgumentException(nameof(binsCount));

            if (minDb == 0)
                throw new ArgumentException(nameof(minDb));

            maxFrequency = Math.Min(maxFrequency, SamplingFrequency);

            float[] data = new float[binsCount];
            double[] frequencies = GenerateLogSpace(minFrequency, size == SpectrumSize.Full ? maxFrequency : maxFrequency / 2, binsCount);

            for (int i = 0; i < frequencies.Length - 1; i++)
            {
                var amplitude = (float)(20 * Math.Log10(this[frequencies[i], frequencies[i + 1]])); // Probably correct visualization
                amplitude = float.IsInfinity(amplitude) ? minDb : amplitude; // Safety check
                amplitude = MathF.Min(MathF.Max(minDb, amplitude), maxDb);
                data[i] = amplitude;
            }

            var topRange = MathF.Log10(Math.Abs(maxDb));
            var bottomRange = MathF.Log10(Math.Abs(minDb));

            for (int i = 0; i < binsCount; i++)
            {
                var value = MathF.Log10(MathF.Abs(data[i]));
                var pos = (value - topRange) / (bottomRange - topRange);
                data[i] = 1 - pos;
            }

            return data;
        }

        private static double[] GenerateLogSpace(int min, int max, int logBins)
        {
            double logarithmicBase = Math.E;
            double logMin = Math.Log(min);
            double logMax = Math.Log(max);
            double delta = (logMax - logMin) / logBins;

            double accDelta = 0;
            double[] v = new double[logBins + 1];
            for (int i = 0; i <= logBins; ++i)
            {
                v[i] = Math.Pow(logarithmicBase, logMin + accDelta);
                accDelta += delta; // accDelta = delta * i
            }

            v[^1] = max;
            return v;
        }

        private static double[] GenerateLogSpace(double min, double max, int logBins)
        {
            double logarithmicBase = Math.E;
            double logMin = Math.Log(min);
            double logMax = Math.Log(max);
            double delta = (logMax - logMin) / logBins;

            double accDelta = 0;
            double[] v = new double[logBins + 1];
            for (int i = 0; i <= logBins; ++i)
            {
                v[i] = Math.Pow(logarithmicBase, logMin + accDelta);
                accDelta += delta; // accDelta = delta * i
            }

            v[^1] = max;
            return v;
        }
    }

    public enum SpectrumSize
    {
        Full,
        Half
    }
}
