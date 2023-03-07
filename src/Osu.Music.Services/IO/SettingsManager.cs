using Newtonsoft.Json;
using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;
using Prism.Mvvm;
using System.IO;

namespace Osu.Music.Services.IO
{
    public class SettingsManager : BindableBase
    {
        private readonly string _settingsFile = Path.Combine(AppDataHelper.Path, "settings.json");

        private Settings _settings;
        public Settings Settings
        {
            get => _settings ?? Load();
            set => SetProperty(ref _settings, value);
        }

        public Settings Load()
        {
            try
            {
                string json = File.Exists(_settingsFile) ? File.ReadAllText(_settingsFile) : null;
                Settings = json != null ? JsonConvert.DeserializeObject<Settings>(json, new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace }) : new Settings();
                return Settings;
            }
            catch
            {
                Settings = new Settings();
                return Settings;
            }
        }

        public void Save(Settings settings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings);
                File.WriteAllText(_settingsFile, json);
            }
            catch
            {
                // Silently fail
            }
        }
    }
}
