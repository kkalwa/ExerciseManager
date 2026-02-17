using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
   public class CreateNewProfileViewModel: BaseViewModel
    {
        /** Public properties used by views
         * 
         **/
        public string NewUserLogin { get; set; } = string.Empty;
        public SecureString NewUserPassword { get; set; } = new();
        public SecureString NewUserPasswordRepeated { get; set; } = new();
        public ICommand ConfirmCreatingProfileCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        /** Functions associated with RelayCommands
         * 
         * */
        private void ConfirmCreatingProfile(object sender)
        {

        }
        private bool CheckIfFormIsFilled(object sender)
        {
            return !string.IsNullOrEmpty(NewUserLogin) &&
                   (NewUserPassword.Length > 0) &&
                   (NewUserPasswordRepeated.Length > 0);
        }
        private void Cancel(object sender)
        {
                viewMediator.ChangeViewTo(new LoginViewModel(viewMediator));
        }

        private byte[] salt = RandomNumberGenerator.GetBytes(32);
        public CreateNewProfileViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            ConfirmCreatingProfileCommand = new RelayCommand(ConfirmCreatingProfile, CheckIfFormIsFilled);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void OnPasswordChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                byte[] encrypted = KeyDerivation.Pbkdf2(
                        password: (sender as TextBox).Text,
                        salt: this.salt,
                        prf: KeyDerivationPrf.HMACSHA512,
                        iterationCount: 100,
                        numBytesRequested: 64);
            }
        }
    }
}
