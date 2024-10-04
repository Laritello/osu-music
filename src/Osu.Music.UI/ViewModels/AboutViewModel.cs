using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Osu.Music.UI.ViewModels
{
	public class AboutViewModel : BindableBase, INavigationAware
	{
		private ObservableCollection<LicenseNotice> _licenses = [];
		public ObservableCollection<LicenseNotice> Licenses
		{
			get => _licenses;
			set => SetProperty(ref _licenses, value);
		}

		private string _version;
		public string Version
		{
			get => _version;
			set => SetProperty(ref _version, value);
		}

		private ObservableCollection<LicenseNotice> _licenseContent;
		public ObservableCollection<LicenseNotice> LicenseContent
		{
			get => _licenseContent;
			set => SetProperty(ref _licenseContent, value);
		}

		public DelegateCommand<string> OpenRepositoryCommand { get; private set; }
		public DelegateCommand OpenReleaseNotesCommand { get; private set; }
		public DelegateCommand OpenLicenseCommand { get; private set; }
		public DelegateCommand OpenNoticesCommand { get; private set; }

		public AboutViewModel()
		{
			InitializeCommands();
			Load();
		}

		private void InitializeCommands()
		{
			OpenRepositoryCommand = new DelegateCommand<string>(OpenRepository);
			OpenReleaseNotesCommand = new DelegateCommand(OpenReleaseNotes);
			OpenLicenseCommand = new DelegateCommand(OpenLicense);
			OpenNoticesCommand = new DelegateCommand(OpenNotices);
		}

		private void Load()
		{
			Version = ReadVersion();
			Licenses = AppDataHelper.GetLicenses();
			LicenseContent = [];
		}

		private void OpenRepository(string url) => Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });

		private void OpenReleaseNotes() => Process.Start(new ProcessStartInfo("cmd", $"/c start {$"https://github.com/Laritello/osu-music/releases/tag/{Version}"}") { CreateNoWindow = true });

		private void OpenLicense()
		{
			if (Licenses?.Count > 0)
			{
				LicenseContent.Clear();
				LicenseContent.Add(Licenses.First());
			}
		}

		private void OpenNotices()
		{
			if (Licenses?.Count > 0)
			{
				LicenseContent.Clear();
				LicenseContent.AddRange(Licenses.Skip(1).ToList());
			}
		}

		private string ReadVersion()
		{
			var version = Assembly.GetEntryAssembly().GetName().Version;
			return $"{version.Major}.{version.Minor}.{version.Build}";
		}

		public void OnNavigatedTo(NavigationContext navigationContext) { }

		public bool IsNavigationTarget(NavigationContext navigationContext) => true;

		public void OnNavigatedFrom(NavigationContext navigationContext) { }
	}
}
