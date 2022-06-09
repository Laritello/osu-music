using Osu.Music.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class BeatmapToAvailablePlaylistsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is Beatmap) || !(values[1] is List<Playlist>))
                return new List<Playlist>();

            var beatmap = values[0] as Beatmap;
            var playlists = values[1] as List<Playlist>;

            return playlists.Where(x => x.Beatmaps.Contains(beatmap)).ToList();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
