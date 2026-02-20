using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Repositories;
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
        /**Fields not associated with properties
         * 
         * */
        private IUserRepository userRepository;

        /**Private fields associated with properties
         * 
         * */
        private String errorString = string.Empty;

        /** Public properties used by views
         * 
         **/
        public string NewUserLogin { get; set; } = string.Empty;
        public String NewUserPassword { get; set; } = string.Empty;
        public String NewUserPasswordRepeated { get; set; } = string.Empty;
        public ICommand ConfirmCreatingProfileCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public String ErrorString
        {
            get { return errorString; }
            set { errorString = value;
                OnPropertyChanged();
            }
        }

        /** Functions associated with RelayCommands
         * 
         * */
        private void ConfirmCreatingProfile(object sender)
        {
            SQLResults result = userRepository.Add(new UserModel { Username = NewUserLogin, Password = NewUserPassword });
            if (result == SQLResults.UserAlreadyExists)
            {
                ErrorString= "Użytkownik o podanej nazwie już istnieje";
            }else if(result == SQLResults.Failure)
            {
                ErrorString = "Błąd zapisu. Sprawdź połączenie z bazą";
            }else if(result == SQLResults.Success) 
            {
                ErrorString = "Pomyślnie utworzono profil. Możesz się teraz zalogować";
            }
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
        /** Constructor
         *
         * */
        public CreateNewProfileViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            ConfirmCreatingProfileCommand = new RelayCommand(ConfirmCreatingProfile);
            CancelCommand = new RelayCommand(Cancel);
            userRepository = new UserRepository();
            ErrorString = "Niech tutaj wyswietla sie status";
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
