using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
   public class CreateNewProfileViewModel: BaseViewModel
    {
        /** Public properties used by views
         * 
         **/
        public string NewUserLogin { get; set; } = string.Empty;
        public string NewUserPassword { get; set; } = string.Empty;
        public string NewUserPasswordRepeated { get; set; } = string.Empty;
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
                   !string.IsNullOrEmpty(NewUserPassword) &&
                   !string.IsNullOrEmpty(NewUserPasswordRepeated) &&
                   NewUserPassword == NewUserPasswordRepeated;
        }
        private void Cancel(object sender)
        {
                viewMediator.ChangeViewTo(new LoginViewModel(viewMediator));
        }


        public CreateNewProfileViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            ConfirmCreatingProfileCommand = new RelayCommand(ConfirmCreatingProfile, CheckIfFormIsFilled);
            CancelCommand = new RelayCommand(Cancel);
        }
    }
}
