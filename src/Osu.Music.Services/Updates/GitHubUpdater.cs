using Osu.Music.Common.Enums;
using Prism.Mvvm;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
    }
}
