using NAudio.Wave;
using Osu.Music.Common.Models;
using Osu.Music.Services.Events;
using Prism.Mvvm;
using System;

namespace Osu.Music.Services.Audio
{
    // Modified https://github.com/naudio/NAudio/blob/master/NAudioWpfDemo/AudioPlaybackDemo/AudioPlayback.cs file
    public class AudioPlayback : BindableBase, IDisposable
    {
        #region Properties
        public Beatmap Beatmap { get; set; }

        private bool _mute = false;
        public bool Mute
        {
            get => _mute;
            set
            {
                SetProperty(ref _mute, value);
                SetMute();
            }
        }

        private float _volume = 0.3f;
        public float Volume
        {
            get => _volume;
            set
            {
                SetProperty(ref _volume, Math.Min(Math.Max(value, 0), 1)); // Volume should be in range [0;1]
                SetVolume();
            }
        }

        private bool _repeat;
        public bool Repeat
        {
            get => _repeat;
            set => SetProperty(ref _repeat, value);
        }

        private bool _shuffle;
        public bool Shuffle
        {
            get => _shuffle;
            set => SetProperty(ref _shuffle, value);
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
            fileStream?.Close();
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
                    PerformFFT = true
                };
                aggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(this, a);
                playbackDevice.Init(aggregator);
            }
            catch
            {
                CloseFile();
            }
        }

        private void EnsureDeviceCreated()
        {
            if (playbackDevice != null)
            {
                playbackDevice.Dispose();
                playbackDevice = null;
            }

            CreateDevice();
        }

        private void CreateDevice()
        {
            playbackDevice = new WaveOut { DesiredLatency = 200, Volume = _mute ? 0 : _volume };

            playbackDevice.PlaybackStopped += (s, a) =>
            {
                // I use difference between TimeSpans because some song are ending before EOF. On average it is less than 50 ms
                // so window in 100 ms should do the work.
                // Downside for this is that if someone will stop the song within 100 ms before it's end program will think that it should play
                // next song. But it should occur extremly rarely. If someone will suggest better way to detect when song ended by itself
                // I'll fix this semi hack.
                if (a.Exception == null && fileStream != null && (fileStream.TotalTime - fileStream.CurrentTime).TotalMilliseconds < 100)
                    BeatmapEnded?.Invoke(this, new BeatmapEventArgs(Beatmap));
            };
        }

        private void SetMute()
        {
            if (playbackDevice != null)
                playbackDevice.Volume = _mute ? 0 : _volume;
        }

        private void SetVolume()
        {
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
}
