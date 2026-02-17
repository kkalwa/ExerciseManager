using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    
    public class LoginViewModel: BaseViewModel
    {

        /** Fields not associated with properties
         * 
         */
        private IUserRepository userRepository;


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
        public string Password { get; set; } = string.Empty;
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
        public ICommand CreateProfileCommand { get; set; }

        /** Functions associated with RelayCommands
         * 
         * */
        private void GoToCreateNewProfile(object obj)
        {
            viewMediator.ChangeViewTo(new CreateNewProfileViewModel(viewMediator));
        }
        private void InitiateLoginProcess(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Login, Password));

            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Login), null);
                viewMediator.ChangeViewTo(new UserPanelViewModel(viewMediator));
            }
            else
            {
            }
        }

        /** Constructor
         * 
         * */
        public LoginViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            userRepository = new UserRepository();
            
            CreateProfileCommand = new RelayCommand(GoToCreateNewProfile);
            InitiateLogInProcessCommand = new RelayCommand(InitiateLoginProcess);
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
