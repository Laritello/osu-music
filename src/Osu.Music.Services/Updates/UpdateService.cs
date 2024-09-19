using Osu.Music.Services.Abstractions;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace Osu.Music.Services.Updates
{
	public class UpdateService : IUpdateService
	{
		private readonly UpdateManager _manager;
		private UpdateInfo _update;

		public UpdateService()
		{
			_manager = new(new GithubSource("https://github.com/Laritello/osu-music", string.Empty, false));
		}

		public async Task<bool> HasUpdate()
		{
			_update = await _manager.CheckForUpdatesAsync();
			return _update != null;
		}

		public async Task Update()
		{
			if (_update == null) return;

			await _manager.DownloadUpdatesAsync(_update);
			_manager.ApplyUpdatesAndRestart(_update);
		}
	}
}
