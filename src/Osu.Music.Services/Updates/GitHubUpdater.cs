using Osu.Music.Common.Enums;
using Prism.Mvvm;
using Squirrel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Osu.Music.Services.Updates
{
    /// <summary>
    /// Wrapper for Squirrel UpdateManager
    /// </summary>
    public class GitHubUpdater : BindableBase
    {
        private UpdateManager _manager;
        public UpdateManager Manager
        {
            get => _manager;
            set => SetProperty(ref _manager, value);
        }

        private UpdateState _state;
        public UpdateState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public GitHubUpdater()
        {
            State = UpdateState.Latest;
        }

        public async void CheckForUpdates()
        {
            if (Manager != null)
            {
                var info = await Manager.CheckForUpdate();
                State = info.ReleasesToApply.Count > 0 ? UpdateState.Available : UpdateState.Latest;
            }
        }

        public async void Update()
        {
            if (Manager != null)
            {
                var updates = await Manager.CheckForUpdate();

                if (updates.ReleasesToApply.Any())
                {
                    State = UpdateState.InProgress;
                    var lastVersion = updates.ReleasesToApply.OrderBy(x => x.Version).Last();

                    await Manager.UpdateApp();

                    var latestExe = Path.Combine(Manager.RootAppDirectory, string.Concat("app-", lastVersion.Version), "Osu.Music.exe");

                    System.Diagnostics.Process.Start(latestExe);
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
