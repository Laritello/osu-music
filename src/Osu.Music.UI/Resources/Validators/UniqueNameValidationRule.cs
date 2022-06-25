using Osu.Music.UI.Resources.Converters;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Osu.Music.UI.Resources.Validators
{
    public class UniqueNameValidationRule : ValidationRule
    {
        private UniqueNameValidationParameters _parameters;
        public UniqueNameValidationParameters Parameters
        {
            get => _parameters;
            set => _parameters = value;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Parameters.Items == null || Parameters.Item == null)
                return ValidationResult.ValidResult;

            var converter = new PlayerObjectToStringConverter();
            var names = Parameters.Items.Where(x => x != Parameters.Item).Select(x => converter.Convert(x, typeof(string), null, cultureInfo));

            return names.Contains((value ?? "").ToString())
                ? new ValidationResult(false, "Field must be unique.")
                : ValidationResult.ValidResult;
        }
    }
}
