using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    
    public class LoginViewModel: BaseViewModel
    {
        /** Private fields associated with properties
         * 
         **/
        private string? login;
        private string? loginStatus;
        private bool startAnimation;

        /** Public properties used by views
         * 
         **/
        public string Login
        {
            get { return login ?? string.Empty; }
            set { login = value; }
        }
        public string LoginStatus
        {
            get { return loginStatus ?? String.Empty; }
            set
            {
                loginStatus = value;
                OnPropertyChanged();
            }
        }
        public bool StartAnimation
        {
            get { return startAnimation; }
            set
            {
                startAnimation = value;
                OnPropertyChanged();
            }
        }
        public string TitleText { get; } = "Zaloguj się";
        public RelayCommand GoToUserPanelCommand { get; set; } //**************************************************** Remove later *****************************
        public ICommand InitiateLogInProcessCommand { get; set; }

        public LoginViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            
        }



        /** Stuff that probably will be removed in the future
         * 
         **/
        private void GoToUserPanel(object obj)
        {
            viewMediator.ChangeViewTo(new UserPanelViewModel(viewMediator));
        }
    }
}
