using DataErrorInfoValidationExample.Dialogs;
using DataErrorInfoValidationExample.ValidationRules;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System.ComponentModel;
using System.Windows;

namespace DataErrorInfoValidationExample.ViewModels.Main
{
    [POCOViewModel]
    public class MainViewModel : IDataErrorInfo
    {
        private bool _oneTimeValidationIsEnable;

        #region PROPERTIES Getters/ Setters

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string PasswordConfirm { get; set; }

        public virtual string Comment { get; set; }

        #endregion

        #region MODEL VALIDATION

        public void ValidationMainViewData()
        {
            string errorMessage = ViewValidation();

            if (errorMessage != null)
            {
                ValidationFailed(errorMessage);

                return;
            }

            ValidationSucceeded();
        }

        private string ViewValidation()
        {
            _oneTimeValidationIsEnable = true;

            string errorMessage = ((IDataErrorInfo)this).Error;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.RaisePropertiesChanged();

                return errorMessage;
            }

            _oneTimeValidationIsEnable = false;

            return null;
        }

        private void ValidationSucceeded()
        {
            string successMessage = "The input data entered were as follows:\n\n" +
                                    "\tUsername: " + UserName + "\n" +
                                    "\tPassword: " + Password + "\n" +
                                    "\tPassword Confirmation: " + PasswordConfirm + "\n" +
                                    "\tComment: " + (!string.IsNullOrEmpty(Comment) ? Comment : "Unknown");

            new NotificationDialog("Validation Success!", successMessage)
                .ShowMessageBoxByMessageType(MessageBoxImage.Information);
        }

        private static void ValidationFailed(string error)
        {
            new NotificationDialog("Validation Error" + error, error)
                .ShowMessageBoxByMessageType(MessageBoxImage.Error);
        }


        #endregion

        #region ERROR HANDLERS

        string IDataErrorInfo.Error
        {
            get
            {
                if (!_oneTimeValidationIsEnable)
                {
                    return null;
                }

                IDataErrorInfo errorInfo = this;

                string errorMessage = errorInfo[BindableBase.GetPropertyName(() => UserName)] +
                                      errorInfo[BindableBase.GetPropertyName(() => Password)] +
                                      errorInfo[BindableBase.GetPropertyName(() => PasswordConfirm)];

                return !string.IsNullOrEmpty(errorMessage) ? "Please check your INPUT data!" : null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!_oneTimeValidationIsEnable)
                {
                    return null;
                }

                if (columnName == BindableBase.GetPropertyName(() => UserName))
                {
                    return MainViewValidationRule.GetErrorMessage(BindableBase.GetPropertyName(() => UserName), UserName);
                }

                if (columnName == BindableBase.GetPropertyName(() => Password))
                {
                    if (PasswordConfirm != null && !Equals(Password, PasswordConfirm))
                    {
                        return "The two passwords do not match";
                    }

                    return MainViewValidationRule.GetErrorMessage(BindableBase.GetPropertyName(() => Password), Password);
                }

                if (columnName == BindableBase.GetPropertyName(() => PasswordConfirm))
                {
                    if (!string.IsNullOrEmpty(Password) && Password != PasswordConfirm)
                    {
                        return "The two passwords do not match!";
                    }

                    return MainViewValidationRule.GetErrorMessage(BindableBase.GetPropertyName(() => PasswordConfirm), PasswordConfirm);
                }

                return null;
            }
        }

        #endregion
    }
}
