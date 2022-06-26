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
        private Player _player;
        public Player Player
        {
            get => _player;
            set => SetProperty(ref _player, value);
        }

        private IWavePlayer _device;
        public IWavePlayer Device
        {
            get => _device;
            set => SetProperty(ref _device, value);
        }

        private WaveStream _stream;
        public WaveStream Stream
        {
            get => _stream;
            set => SetProperty(ref _stream, value);
        }
        #endregion

        #region Events
        public event EventHandler<BeatmapEventArgs> BeatmapEnded;
        public event EventHandler<FftEventArgs> FftCalculated;
        #endregion

        public AudioPlayback(Player player)
        {
            Player = player;
        }

        public void Load()
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile();
        }

        private void CloseFile()
        {
            Stream?.Close();
            Stream?.Dispose();
            Stream = null;
        }

        private void OpenFile()
        {
            try
            {
                AudioFileReader inputStream = new AudioFileReader(Player.Beatmap.AudioFilePath);
                Stream = inputStream;

                SampleAggregator aggregator = new SampleAggregator(inputStream)
                {
                    PerformFFT = true
                };
                aggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(this, a);
                Device.Init(aggregator);
            }
            catch
            {
                CloseFile();
            }
        }

        private void EnsureDeviceCreated()
        {
            if (Device != null)
            {
                Device.Dispose();
                Device = null;
            }

            CreateDevice();
        }

        private void CreateDevice()
        {
            Device = new WaveOut { DesiredLatency = 200, Volume = Player.Mute ? 0 : Player.Volume };

            Device.PlaybackStopped += (s, a) =>
            {
                // I use difference between TimeSpans because some song are ending before EOF. On average it is less than 50 ms
                // so window in 100 ms should do the work.
                // Downside for this is that if someone will stop the song within 100 ms before it's end program will think that it should play
                // next song. But it should occur extremly rarely. If someone will suggest better way to detect when song ended by itself
                // I'll fix this semi hack.
                if (a.Exception == null && Stream != null && (Stream.TotalTime - Stream.CurrentTime).TotalMilliseconds < 100)
                    BeatmapEnded?.Invoke(this, new BeatmapEventArgs(Player.Beatmap));
            };
        }

        public void SetMute()
        {
            if (Device != null)
                Device.Volume = Player.Mute ? 0 : Player.Volume;
        }

        public void SetVolume()
        {
            if (Device != null)
                Device.Volume = Player.Volume;
        }

        public void Play()
        {
            if (Device != null && Stream != null && Device.PlaybackState != PlaybackState.Playing)
                Device.Play();
        }

        public void Pause() => Device?.Pause();

        public void Stop()
        {
            Device?.Stop();

            if (Stream != null)
                Stream.Position = 0;
        }

        public void Dispose()
        {
            Stop();
            CloseFile();
            Device?.Dispose();
            Device = null;
        }
    }
}
