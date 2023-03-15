using Prism.Mvvm;

namespace Osu.Music.Services.Localization
{
    public class LocalizationCulture : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _code;
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        private string _culture;
        public string Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }

        // TODO: Add license on FAMFAMFAM library
        public LocalizationCulture(string name, string code, string culture)
        {
            Name = name;
            Code = code;
            Culture = culture;
        }
    }
}
