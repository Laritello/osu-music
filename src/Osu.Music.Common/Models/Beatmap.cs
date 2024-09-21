using Newtonsoft.Json;
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
		#region Backgind fields

		private int _beatmapSetId;
		private string _title;
		private string _titleUnicode;
		private string _artist;
		private string _artistUnicode;
		private string _creator;
		private string _audioFileName;
		private TimeSpan _totalTime;
		private string _tags;
		private string _directory;
		private string _fileName;
		private ICollection<string> _hashes = [];
		private string _backgroundFilePath;
		private int _matches;

		#endregion

		#region Properties

		/// <summary>
		/// Beatmap ID.
		/// </summary>
		public int BeatmapSetId
		{
			get => _beatmapSetId;
			set => SetProperty(ref _beatmapSetId, value);
		}

		/// <summary>
		/// Romanised song title.
		/// </summary>
		[JsonIgnore]
		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		/// <summary>
		/// Song title.
		/// </summary>
		[JsonIgnore]
		public string TitleUnicode
		{
			get => _titleUnicode;
			set => SetProperty(ref _titleUnicode, value);
		}

		/// <summary>
		/// Romanised song artist.
		/// </summary>
		[JsonIgnore]
		public string Artist
		{
			get => _artist;
			set => SetProperty(ref _artist, value);
		}

		/// <summary>
		/// Song artist.
		/// </summary>
		[JsonIgnore]
		public string ArtistUnicode
		{
			get => _artistUnicode;
			set => SetProperty(ref _artistUnicode, value);
		}

		/// <summary>
		/// Beatmap creator.
		/// </summary>
		[JsonIgnore]
		public string Creator
		{
			get => _creator;
			set => SetProperty(ref _creator, value);
		}

		/// <summary>
		/// Location of the audio file.
		/// </summary>
		[JsonIgnore]
		public string AudioFileName
		{
			get => _audioFileName;
			set => SetProperty(ref _audioFileName, value);
		}

		/// <summary>
		/// Total duration of the audio file.
		/// </summary>
		[JsonIgnore]
		public TimeSpan TotalTime
		{
			get => _totalTime;
			set => SetProperty(ref _totalTime, value);
		}

		/// <summary>
		/// Space-separated list of search terms.
		/// </summary>
		[JsonIgnore]
		public string Tags
		{
			get => _tags;
			set => SetProperty(ref _tags, value);
		}

		/// <summary>
		/// Location of the beatmap.
		/// </summary>
		[JsonIgnore]
		public string Directory
		{
			get => _directory;
			set => SetProperty(ref _directory, value);
		}

		/// <summary>
		/// Name of the .osu file
		/// </summary>
		[JsonIgnore]
		public string FileName
		{
			get => _fileName;
			set => SetProperty(ref _fileName, value);
		}

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
		/// Full path to background image file.
		/// </summary>
		[JsonIgnore]
		public string BackgroundFilePath => _backgroundFilePath ??= BackgroundRepository.GetImagePath(this);

		/// <summary>
		/// Full path to audio file.
		/// </summary>
		[JsonIgnore]
		public string AudioFilePath => Path.Combine(Directory, AudioFileName);


		/// <summary>
		/// The amount of found matches during search.
		/// </summary>
		[JsonIgnore]
		public int Matches
		{
			get => _matches;
			private set => SetProperty(ref _matches, value);
		}

		#endregion

		#region Methods

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

		#endregion

	}
}
