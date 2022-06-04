using NAudio.Dsp;
using NAudio.Wave;
using Osu.Music.Services.Events;
using System;

namespace Osu.Music.Services.Audio
{
    /// <summary>
    /// Demo sample provider that performs FFTs
    /// </summary>
    public class SampleAggregator : ISampleProvider
    {
        // FFT
        public event EventHandler<FftEventArgs> FftCalculated;
        public bool PerformFFT { get; set; }
        private readonly Complex[] fftBuffer;
        private int fftPos;
        private readonly int fftLength;
        private readonly int m;
        private readonly ISampleProvider source;

        private readonly int channels;

        private int sampleCount;

        public SampleAggregator(ISampleProvider source, int fftLength = 4096)
        {
            if (!IsPowerOfTwo(fftLength))
                throw new ArgumentException("FFT Length must be a power of two");

            channels = source.WaveFormat.Channels;

            m = (int)Math.Log(fftLength, 2.0);
            this.fftLength = fftLength;
            this.source = source;

            fftBuffer = new Complex[fftLength];
        }

        private static bool IsPowerOfTwo(int x) => (x & (x - 1)) == 0;

        private void Add(float value)
        {
            if (PerformFFT && FftCalculated != null)
            {
                fftBuffer[fftPos].X = (float)(value * FastFourierTransform.HammingWindow(fftPos, fftLength));
                fftBuffer[fftPos].Y = 0;
                fftPos++;

                if (fftPos >= fftBuffer.Length)
                {
                    fftPos = 0;
                    FastFourierTransform.FFT(true, m, fftBuffer);
                    FftCalculated(this, new FftEventArgs(fftBuffer, source.WaveFormat.SampleRate, fftLength));
                }
            }
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            try
            {
                sampleCount = source.Read(buffer, offset, count);

                for (int n = 0; n < sampleCount; n += channels)
                    Add(buffer[n + offset]);

                return sampleCount;
            }
            catch
            {
                return 0;
            }
        }
    }
}
