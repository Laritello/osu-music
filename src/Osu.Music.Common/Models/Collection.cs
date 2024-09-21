using Newtonsoft.Json;
using Osu.Music.Common.Interfaces;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Osu.Music.Common.Models
{
	public class Collection : BindableBase, ISearchable
	{
		#region Backing fields

		private string _name;
		private ObservableCollection<Beatmap> _beatmaps = [];
		private int _matches;

		#endregion


		#region Properties
		/// <summary>
		/// The name of the the collection.
		/// </summary>
		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		/// <summary>
		/// The beatmaps of the collection.
		/// </summary>
		public ObservableCollection<Beatmap> Beatmaps
		{
			get => _beatmaps;
			set => SetProperty(ref _beatmaps, value);
		}

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

		public bool Match(Regex query)
		{
			Matches = query.Matches(Name).Count;
			return Matches > 0;
		}

		public string GetNavigationView() => "CollectionDetailsView";

		#endregion
	}
}
