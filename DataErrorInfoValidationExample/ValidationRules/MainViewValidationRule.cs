using System.Globalization;
using System.Windows.Controls;

namespace DataErrorInfoValidationExample.ValidationRules
{
    public class MainViewValidationRule : ValidationRule
    {
        public string FieldName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string errorMessage = GetErrorMessage(FieldName, value);

            return !string.IsNullOrEmpty(errorMessage) ? new ValidationResult(false, errorMessage) : ValidationResult.ValidResult;
        }

        public static string GetErrorMessage(string fieldName, object fieldValue)
        {
            string errorMessage = string.Empty;

            switch (fieldName.ToLower())
            {
                case "username":
                    errorMessage += UserNameValidation(fieldValue);
                    break;
                case "password":
                case "passwordconfirm":
                    errorMessage += PasswordValidation(fieldName, fieldValue);
                    break;
            }

            return errorMessage;
        }

        private static string UserNameValidation(object fieldValue)
        {
            return string.IsNullOrEmpty(fieldValue?.ToString()) ? "Please, enter your Username!" : string.Empty;
        }

        private static string PasswordValidation(string fieldName, object fieldValue)
        {
            const int passwordMinLength = 8;

            switch (fieldName.ToLower())
            {
                case "password" when string.IsNullOrEmpty(fieldValue?.ToString()):
                    return "Please, enter your Password!";
                case "passwordconfirm" when string.IsNullOrEmpty(fieldValue?.ToString()):
                    return "Please, enter your Password again!";
                case "password":
                    return fieldValue.ToString().Length < passwordMinLength ?
                        $"Password must be at least {passwordMinLength} characters long" :
                        string.Empty;
                case "passwordconfirm":
                    return fieldValue.ToString().Length < passwordMinLength ?
                        $"Password must be at least {passwordMinLength} characters long" :
                        string.Empty;
                default:
                    return string.Empty;
            }
        }
    }
}