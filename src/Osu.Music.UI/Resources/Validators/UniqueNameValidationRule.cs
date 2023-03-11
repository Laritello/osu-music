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
            var name = (string)value;

            if (Parameters.Names == null || string.IsNullOrEmpty(name))
                return ValidationResult.ValidResult;

            return Parameters.Names.Contains((value ?? "").ToString())
                ? new ValidationResult(false, "Field must be unique.")
                : ValidationResult.ValidResult;
        }
    }
}
