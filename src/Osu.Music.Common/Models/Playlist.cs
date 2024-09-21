using Newtonsoft.Json;
using Osu.Music.Common.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Osu.Music.Common.Models
{
	public class Playlist : BindableBase, ISearchable
	{
		#region Backing fields

		private string _name;
		private DateTime _updated;
		private ObservableCollection<Beatmap> _beatmaps = [];
		private int _matches;

		#endregion

		#region Properties

		/// <summary>
		/// Name of the playlist.
		/// </summary>
		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		/// <summary>
		/// Last time playlist was updated.
		/// </summary>
		public DateTime Updated
		{
			get => _updated;
			set => SetProperty(ref _updated, value);
		}

		/// <summary>
		/// Beatmaps of the playlist.
		/// </summary>
		public ObservableCollection<Beatmap> Beatmaps
		{
			get => _beatmaps;
			set => SetProperty(ref _beatmaps, value);
		}

		/// <summary>
		/// Amount of found matches during search.
		/// </summary>
		[JsonIgnore]
		public int Matches
		{
			get => _matches;
			private set => SetProperty(ref _matches, value);
		}

		#endregion

		#region Methods

		public void UpdateMaps(ICollection<Beatmap> beatmaps)
		{
			var id = Beatmaps.Select(x => x.BeatmapSetId).ToList();
			var playlistBeatmaps = beatmaps.Where(x => id.Contains(x.BeatmapSetId)).ToList();

			for (int i = 0; i < Beatmaps.Count; i++)
			{
				Beatmaps[i] = playlistBeatmaps.Where(x => x.BeatmapSetId == Beatmaps[i].BeatmapSetId).FirstOrDefault() ?? Beatmaps[i];
			}
		}

		public bool Match(Regex query)
		{
			Matches = query.Matches(Name).Count;
			return Matches > 0;
		}

		public string GetNavigationView() => "PlaylistDetailsView";

		#endregion
	}
}
