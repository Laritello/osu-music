using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Osu.Music.Common.Models
{
    public class Beatmap
    {
        /// <summary>
        /// Beatmap ID.
        /// </summary>
        public int BeatmapSetID { get; set; }

        /// <summary>
        /// Romanised song title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Song title.
        /// </summary>
        public string TitleUnicode { get; set; }

        /// <summary>
        /// Romanised song artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Song artist.
        /// </summary>
        public string ArtistUnicode { get; set; }

        /// <summary>
        /// Beatmap creator.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Location of the audio file.
        /// </summary>
        public string AudioFileName { get; set; }

        /// <summary>
        /// Total duration of the audio file.
        /// </summary>
        public TimeSpan TotalTime { get; set; }

        /// <summary>
        /// Location of the background image file.
        /// </summary>
        public string BackgroundFileName { get; set; }

        /// <summary>
        /// Space-separated list of search terms.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Location of the beatmap.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Full path to audio file.
        /// </summary>
        public string AudioFilePath { get => Path.Combine(Directory, AudioFileName); }

        /// <summary>
        /// Full path to background image file.
        /// </summary>
        public string BackgroundFilePath { get =>  (Directory == null || BackgroundFileName == null) ? "" : Path.Combine(Directory, BackgroundFileName); }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Beatmap b = (Beatmap)obj;
                return BeatmapSetID == b.BeatmapSetID && Title == b.Title && Artist == b.Artist && Creator == b.Creator; // Leave only ID check?
            }
        }
    }
}
