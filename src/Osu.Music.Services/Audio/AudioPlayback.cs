using NAudio.Wave;
using Osu.Music.Common.Models;
using System;

namespace Osu.Music.Services.Audio
{
    // Modified https://github.com/naudio/NAudio/blob/master/NAudioWpfDemo/AudioPlaybackDemo/AudioPlayback.cs file
    public class AudioPlayback : IDisposable
    {
        #region Properties
        public Beatmap Beatmap { get; set; }

        private float _volume = 0.3f;
        public float Volume
        {
            get => _volume;
            set => SetVolume(value);
        }

        public TimeSpan CurrentTime
        {
            get => fileStream != null ? fileStream.CurrentTime : TimeSpan.Zero;
            set
            {
                if (fileStream != null)
                    fileStream.CurrentTime = value;
            }
        }

        public TimeSpan TotalTime => fileStream != null ? fileStream.TotalTime : TimeSpan.Zero;

        public long Position
        {
            get => fileStream != null ? fileStream.Position : 0;
            set
            {
                if (fileStream != null)
                    fileStream.Position = value;
            }
        }
        public long Length => fileStream != null ? fileStream.Length : 0;

        public PlaybackState PlaybackState => playbackDevice != null ? playbackDevice.PlaybackState : PlaybackState.Stopped;
        #endregion

        #region Events
        public event EventHandler<BeatmapEventArgs> BeatmapEnded;
        public event EventHandler<FftEventArgs> FftCalculated;
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;
        #endregion

        #region Private variables
        private IWavePlayer playbackDevice;
        private WaveStream fileStream;
        #endregion

        public void Load()
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile();
        }

        private void CloseFile()
        {
            fileStream?.Dispose();
            fileStream = null;
        }

        private void OpenFile()
        {
            try
            {
                AudioFileReader inputStream = new AudioFileReader(Beatmap.AudioFilePath);
                fileStream = inputStream;

                SampleAggregator aggregator = new SampleAggregator(inputStream)
                {
                    NotificationCount = inputStream.WaveFormat.SampleRate / 100,
                    PerformFFT = true
                };
                aggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(this, a);
                aggregator.MaximumCalculated += (s, a) => MaximumCalculated?.Invoke(this, a);
                playbackDevice.Init(aggregator);
            }
            catch
            {
                CloseFile();
            }
        }

        private void EnsureDeviceCreated()
        {
            if (playbackDevice == null)
                CreateDevice();
        }

        private void CreateDevice()
        {
            playbackDevice = new WaveOut { DesiredLatency = 200, Volume = _volume };

            playbackDevice.PlaybackStopped += (s,a) => {
                if (a.Exception == null)
                    BeatmapEnded?.Invoke(this, new BeatmapEventArgs(Beatmap));
            };
        }

        private void SetVolume(float value)
        {
            _volume = Math.Min(Math.Max(value, 0), 1); // Volume shoild be in range [0;1]

            if (playbackDevice != null)
                playbackDevice.Volume = _volume;
        }

        public void Play()
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
                playbackDevice.Play();
        }

        public void Pause() => playbackDevice?.Pause();

        public void Stop()
        {
            playbackDevice?.Stop();

            if (fileStream != null)
                fileStream.Position = 0;
        }

        public void Dispose()
        {
            Stop();
            CloseFile();
            playbackDevice?.Dispose();
            playbackDevice = null;
        }
    }

    public class BeatmapEventArgs : EventArgs
    {
        public BeatmapEventArgs(Beatmap beatmap)
        {
            Beatmap = beatmap;
        }
        public Beatmap Beatmap { get; private set; }
    }
}
