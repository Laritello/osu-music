﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Osu.Music.Services.Localization
{
    public class LocalizationManager : BindableBase
    {
        private static LocalizationManager _instance;
        public static LocalizationManager Instance => _instance ??= new LocalizationManager();

        private ICollection<LocalizationCulture> _cultures;
        public ICollection<LocalizationCulture> Cultures
        {
            get => _cultures;
            set => SetProperty(ref _cultures, value);
        }

        private LocalizationCulture _culture;
        public LocalizationCulture Culture
        {
            get => _culture;
            set
            {
                SetProperty(ref _culture, value);
                UpdateCulture(_culture);
            }
        }

        public delegate void CultureChangedEventHandler(LocalizationCulture culture);
        public event CultureChangedEventHandler CultureChanged;

        private ResourceDictionary _localizationResource;

        private LocalizationManager()
        {
            Cultures = LocalizationFactory.GetAvailableCultures();
        }

        public string GetLocalizedString(string key)
        {
            return _localizationResource[key] as string;
        }

        private void UpdateCulture(LocalizationCulture culture)
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            // Find existing dictionary with current locale and remove it
            var regex = new Regex("osu.Music.([a-z]{2})-([A-Z]{2}).xaml");
            var oldLocale = dictionaries.FirstOrDefault(x => regex.IsMatch(string.IsNullOrEmpty(x.Source?.OriginalString) ? string.Empty : x.Source?.OriginalString));

            if (oldLocale != null)
                dictionaries.Remove(oldLocale);

            // Append dictionary with new locale
            var currentLocale = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Osu.Music.UI;component/Resources/Language/osu.Music.{culture.Culture}.xaml")
            };

            dictionaries.Add(currentLocale);
            _localizationResource = currentLocale; // Save reference to the current locale so we can access it from ViewModel

            CultureChanged?.Invoke(_culture);
        }
    }
}
