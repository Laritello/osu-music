using Newtonsoft.Json;
using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;
using System.IO;

namespace Osu.Music.Services.IO
{
    public static class SettingsManager
    {
        private static readonly string _settingsFile = Path.Combine(AppDataHelper.Path, "settings.json");
        public static Settings Load()
        {
            try
            {
                string json = File.Exists(_settingsFile) ? File.ReadAllText(_settingsFile) : null;
                return json != null ? JsonConvert.DeserializeObject<Settings>(json, new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace }) : new Settings();
            }
            catch
            {
                return new Settings();
            }
        }

        public static void Save(Settings settings)
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
