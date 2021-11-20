using System;
using System.Windows;

namespace Osu.Music.Services.Events
{
    public class BeatmapProgressBarProgressChangedEventArgs : RoutedEventArgs
    {
        public TimeSpan Progress { get; }

        public BeatmapProgressBarProgressChangedEventArgs(RoutedEvent routedEvent, TimeSpan progress) : base(routedEvent)
        {
            Progress = progress;
        }
    }
}
