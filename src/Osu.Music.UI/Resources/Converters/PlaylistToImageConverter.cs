using NuGet;
using Osu.Music.Common.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class PlaylistToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var playlist = value as Playlist;
            if (playlist.Beatmaps.Count == 0) return null;
            return playlist.Beatmaps[0].BackgroundFilePath == string.Empty 
                ? null 
                : playlist.Beatmaps[0].BackgroundFilePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
