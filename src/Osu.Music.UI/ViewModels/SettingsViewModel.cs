using Osu.Music.UI.UserControls;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osu.Music.UI.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private HotkeyListItem _selectedHotkey;
        public HotkeyListItem SelectedHotkey
        {
            get => _selectedHotkey;
            set => SetProperty(ref _selectedHotkey, value);
        }

        public DelegateCommand<HotkeyListItem> SelectHotkeyCommand { get; private set; }

        public SettingsViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SelectHotkeyCommand = new DelegateCommand<HotkeyListItem>(SelectHotkey);
        }

        private void SelectHotkey(HotkeyListItem hotkey)
        {
            SelectedHotkey = SelectedHotkey == hotkey ? null : hotkey;
        }
    }
}
