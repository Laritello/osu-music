using System.Collections.Generic;

namespace Osu.Music.Services.Localization
{
    public static class LocalizationFactory
    {
        private static Dictionary<string, LocalizationCulture> _cultures = new Dictionary<string, LocalizationCulture>()
        {
            { "en-UK",  new LocalizationCulture("English, UK", "gb", "en-UK") },
            { "en-US",  new LocalizationCulture("English, US", "us", "en-US") },
            { "ru-RU",  new LocalizationCulture("Русский", "ru", "ru-RU") }
        };

        public static LocalizationCulture GetCulture(string culture) => _cultures[culture] ?? _cultures["en-US"];

        public static ICollection<LocalizationCulture> GetAvailableCultures() => _cultures.Values;
    }
}
