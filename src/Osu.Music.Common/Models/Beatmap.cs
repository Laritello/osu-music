﻿using Newtonsoft.Json;
using Osu.Music.Common.Interfaces;
using Osu.Music.Common.Utility;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Osu.Music.Common.Models
{
    public class Beatmap : BindableBase, ISearchable
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

        private string _backgroundFilePath;
        /// <summary>
        /// Full path to background image file.
        /// </summary>
        [JsonIgnore]
        public string BackgroundFilePath
        {
            get
            {
                if (_backgroundFilePath != null) return _backgroundFilePath;
                _backgroundFilePath = BackgroundRepository.GetImagePath(this);
                return _backgroundFilePath;
            }
        }

        /// <summary>
        /// Full path to audio file.
        /// </summary>
        [JsonIgnore]
        public string AudioFilePath { get => Path.Combine(Directory, AudioFileName); }

        private int _matches;
        /// <summary>
        /// The amount of found matches during search.
        /// </summary>
        [JsonIgnore]
        public int Matches
        {
            get => _matches;
            private set => SetProperty(ref _matches, value);
        }

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

        public bool Match(Regex query)
        {
            Matches = query.Matches(Title).Count + query.Matches(Artist).Count;
            return Matches > 0;
        }

        public string GetNavigationView() => "LibraryView";

        public Beatmap()
        {
            Hashes = new List<string>();
        }
    }
}
