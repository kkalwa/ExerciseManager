using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    public class UserPanelViewModel: BaseViewModel
    {
        /** Fields not associated with properties
         * 
         * */
        private UserModel currentLoggedUser;
        /** Private fields associated with properties
         *
         **/
        private UserControl currentUserManageExercises;
        private UserControl currentUserHistory;
        private BaseViewModel currentUserControl;
        private String currentUserName;
        private String currentUserId;
        /**Properties
         * 
         * */
        public UserControl CurrentUserManageExercises
        {
            get { return currentUserManageExercises; }
            set
            {
                currentUserManageExercises = value;
                OnPropertyChanged();
            }
        }
        public UserControl CurrentUserHistory
        {
            get { return currentUserHistory; }
            set
            {
                currentUserHistory = value;
                OnPropertyChanged();
            }
        }
        public BaseViewModel CurrentUserControl
        {
            get { return currentUserControl; }
            set { currentUserControl = value;
                OnPropertyChanged();
            }
        }
        public ICommand ManageExercisesCommand { get; set; }
        public ICommand CheckHistoryCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }
        public String CurrentUserName
        {
            get { return currentUserName; }
            set
            {
                currentUserName = value;
                OnPropertyChanged();
            }
        }

        /* Constructor
         * 
         */
        public UserPanelViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            LogOutCommand = new RelayCommand(LogOut);
            ManageExercisesCommand = new RelayCommand(ManageExercises);
            CheckHistoryCommand = new RelayCommand(CheckHistory);
            currentLoggedUser = viewMediator.CurrentUser;
            currentUserName = viewMediator.CurrentUser.Username;
            currentUserId = viewMediator.CurrentUser.Id;
        }

        private void CheckHistory(object obj)
        {
            CurrentUserControl = new HistoryViewModel(viewMediator);
        }

        private void ManageExercises(object obj)
        {
            CurrentUserControl = new ManageExercisesViewModel(viewMediator);
        }

        private void LogOut(object obj)
        {
            viewMediator.ChangeViewTo(new LoginViewModel(viewMediator));
        }
    }
}
