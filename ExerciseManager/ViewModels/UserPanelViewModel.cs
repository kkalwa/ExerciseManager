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
    public class UserPanelViewModel : BaseViewModel,
        IViewModelParent
    {
        /** Fields not associated with properties
         * 
         * */
        private UserModel currentLoggedUser;
        private Dictionary<string, object> dataStorage = new();
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
        public ICommand AddNewTrainingCommand { get; set; }
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
        public UserPanelViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            LogOutCommand = new RelayCommand(LogOut);
            ManageExercisesCommand = new RelayCommand(ManageExercises);
            CheckHistoryCommand = new RelayCommand(CheckHistory);
            AddNewTrainingCommand = new RelayCommand(AddNewTraining);
            currentLoggedUser = viewMediator.CurrentUser;
            currentUserName = viewMediator.CurrentUser.Username;
            currentUserId = viewMediator.CurrentUser.Id;
        }

        private void CheckHistory(object obj)
        {
            viewMediator.ViewModelParent = this;
            CurrentUserControl = new HistoryViewModel(viewMediator);
        }

        private void ManageExercises(object obj)
        {
            viewMediator.ViewModelParent = this;
            CurrentUserControl = new ManageExercisesViewModel(viewMediator);
        }

        private void LogOut(object obj)
        {
            viewMediator.ViewModelParent = null;
            viewMediator.ChangeViewTo(new LoginViewModel(viewMediator));
        }

        private void AddNewTraining(object obj)
        {
            viewMediator.ViewModelParent = this;
            CurrentUserControl = new ManageTrainingsViewModel(viewMediator);
        }

        public void ChangeChildViewModel(string nameOfNewViewModel)
        {
            switch (nameOfNewViewModel)
            {
                case "ManageExercisesViewModel":
                    CurrentUserControl = new ManageExercisesViewModel(viewMediator);
                    break;
                case "HistoryViewModel":
                    CurrentUserControl = new HistoryViewModel(viewMediator);
                    break;
                case "CreateNewSetViewModel":
                    CurrentUserControl = new CreateNewSetViewModel(viewMediator);
                    break;
                case "CurrentTrainingViewModel":
                    CurrentUserControl = new CurrentTrainingViewModel(viewMediator);
                break;
            }
        }

        public void StoreDataFromChildViewModel(string nameOfData, object data)
        {
            if (string.IsNullOrEmpty(nameOfData))
            {
                throw new ArgumentException("Argument nameOfData is null or empty");
            }
            if (dataStorage.ContainsKey(nameOfData))
            {
                dataStorage[nameOfData] = data;

            } else if (data == null  || nameOfData == null )
            {
                throw new ArgumentNullException("Arguments nameOfData or data are null");
            }else
            {
                dataStorage.Add(nameOfData, data);
            }
            
        }

        public object RetrieveData(string nameOfData)
        {
            if (string.IsNullOrEmpty(nameOfData)) {
                throw new ArgumentException("Argument nameOfData is null or empty");
            } else if (!dataStorage.ContainsKey(nameOfData))
            {
                throw new KeyNotFoundException("Data with such key does not exist");
            }
            return dataStorage[nameOfData];
        }

        public void DeleteData(string nameOfData)
        {
            if ((string.IsNullOrEmpty(nameOfData)))
            {
                throw new ArgumentException("Argument nameOfData is null or empty");
            }
            else if (!dataStorage.ContainsKey(nameOfData))
            {
                throw new KeyNotFoundException("Data with such key does not exist");
            }
            dataStorage.Remove(nameOfData);
        }
    }
}
