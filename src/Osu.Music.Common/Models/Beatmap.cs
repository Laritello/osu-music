using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;

namespace Osu.Music.Common.Models
{
    public class Beatmap : BindableBase
    {
        private int _beatmapSetId;
        /// <summary>
        /// Beatmap ID.
        /// </summary>
        public int BeatmapSetId
        {
            get => _beatmapSetId;
            set => SetProperty(ref _beatmapSetId, value);
        }

        private string _title;
        /// <summary>
        /// Romanised song title.
        /// </summary>
        [JsonIgnore]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _titleUnicode;
        /// <summary>
        /// Song title.
        /// </summary>
        [JsonIgnore]
        public string TitleUnicode
        {
            get => _titleUnicode;
            set => SetProperty(ref _titleUnicode, value);
        }

        private string _artist;
        /// <summary>
        /// Romanised song artist.
        /// </summary>
        [JsonIgnore]
        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        private string _artistUnicode;
        /// <summary>
        /// Song artist.
        /// </summary>
        [JsonIgnore]
        public string ArtistUnicode
        {
            get => _artistUnicode;
            set => SetProperty(ref _artistUnicode, value);
        }

        private string _creator;
        /// <summary>
        /// Beatmap creator.
        /// </summary>
        [JsonIgnore]
        public string Creator
        {
            get => _creator;
            set => SetProperty(ref _creator, value);
        }

        private string _audioFileName;
        /// <summary>
        /// Location of the audio file.
        /// </summary>
        [JsonIgnore]
        public string AudioFileName
        {
            get => _audioFileName;
            set => SetProperty(ref _audioFileName, value);
        }

        private TimeSpan _totalTime;
        /// <summary>
        /// Total duration of the audio file.
        /// </summary>
        [JsonIgnore]
        public TimeSpan TotalTime
        {
            get => _totalTime;
            set => SetProperty(ref _totalTime, value);
        }

        private string _backgroundFileName;
        /// <summary>
        /// Location of the background image file.
        /// </summary>
        [JsonIgnore]
        public string BackgroundFileName
        {
            get => _backgroundFileName;
            set => SetProperty(ref _backgroundFileName, value);
        }

        private string _tags;
        /// <summary>
        /// Space-separated list of search terms.
        /// </summary>
        [JsonIgnore]
        public string Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        private string _directory;
        /// <summary>
        /// Location of the beatmap.
        /// </summary>
        [JsonIgnore]
        public string Directory
        {
            get => _directory;
            set => SetProperty(ref _directory, value);
        }

        private string _fileName;
        /// <summary>
        /// Name of the .osu file
        /// </summary>
        [JsonIgnore]
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private ICollection<string> _hashes;
        /// <summary>
        /// Hash of the .osu file
        /// </summary>
        [JsonIgnore]
        public ICollection<string> Hashes
        {
            get => _hashes;
            set => SetProperty(ref _hashes, value);
        }

        /// <summary>
        /// Full path to audio file.
        /// </summary>
        [JsonIgnore]
        public string AudioFilePath { get => Path.Combine(Directory, AudioFileName); }

        /// <summary>
        /// Full path to background image file.
        /// </summary>
        [JsonIgnore]
        public string BackgroundFilePath { get => (Directory == null || BackgroundFileName == null) ? "" : Path.Combine(Directory, BackgroundFileName); }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Beatmap b = (Beatmap)obj;
                return BeatmapSetId == b.BeatmapSetId && Title == b.Title && Artist == b.Artist && Creator == b.Creator; // Leave only ID check?
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //public NotifyTaskCompletion<Bitmap> Image { get; private set; }

        public Beatmap()
        {
            Hashes = new List<string>();
            //Image = new NotifyTaskCompletion<Bitmap>(BackgroundRepository.GetImageAsync(this));
        }
    }
}
