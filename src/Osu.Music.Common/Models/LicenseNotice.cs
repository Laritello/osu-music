using Prism.Mvvm;

namespace Osu.Music.Common.Models
{
    public class LicenseNotice : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _license;
        public string License
        {
            get => _license;
            set => SetProperty(ref _license, value);
        }

        private string _repository;
        public string Repository
        {
            get => _repository;
            set => SetProperty(ref _repository, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}
