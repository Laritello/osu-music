using System;
using System.Windows;

namespace Osu.Music.Services.Events
{
	public class BeatmapProgressBarProgressChangedEventArgs(RoutedEvent routedEvent, TimeSpan progress) : RoutedEventArgs(routedEvent)
	{
		public TimeSpan Progress { get; } = progress;
	}
}
