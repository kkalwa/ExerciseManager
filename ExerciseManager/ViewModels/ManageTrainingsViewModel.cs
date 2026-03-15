using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    public class ManageTrainingsViewModel: BaseViewModel
    {
        private ObservableCollection<ExerciseSetModel> exerciseSetsList;
        public ObservableCollection<ExerciseSetModel> ExerciseSetsList
        {
            get { return exerciseSetsList; }
            set
            {
                exerciseSetsList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ExerciseSetModel> selectedExerciseSetsList = new();
        public ObservableCollection<ExerciseSetModel> SelectedExerciseSetsList
        {
            get { return selectedExerciseSetsList; }
            set
            {
                selectedExerciseSetsList = value;
                OnPropertyChanged();
            }
        }

        private ExerciseSetModel selectedItem;
        public ExerciseSetModel SelectedItem
            {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

       
        public DBRepository DBRepository { get; private set; }

        public ICommand AddToTrainingCommand { get; set; }
        public ICommand ConfirmTrainingCommand { get; set; }

        
        public ManageTrainingsViewModel(ViewMediator viewMediator) : base(viewMediator) 
        {
            DBRepository = new DBRepository();
            AddToTrainingCommand = new RelayCommand(AddToTraining);
            ConfirmTrainingCommand =  new RelayCommand(ConfirmTraining);
            PrepareExerciseSetsList();
        }
        
        private void AddToTraining(object obj)
        {
            if (SelectedItem != null)
            {
                SelectedExerciseSetsList.Add(SelectedItem);
                SelectedItem = null;
            }
            
        }

        private void ConfirmTraining(object obj)
        {
            viewMediator.StoreData("SELECTED_SETS", SelectedExerciseSetsList);
            viewMediator.ViewModelParent.ChangeChildViewModel("CurrentTrainingViewModel");
        }

        private void PrepareExerciseSetsList()
        {
            List<ManageExercisesModel> list = DBRepository.GetExercisesBasedOnUserId(viewMediator.CurrentUser.Id);
            List<string> listOfSetIds = DBRepository.GetDistinctIdSetsForUser(viewMediator.CurrentUser.Id);
            ObservableCollection<ExerciseSetModel> listOfExerciseSets = new();

            foreach (var element in listOfSetIds)
            {
                ExerciseSetModel newEntry = new() { IdExerciseSet = element };

                foreach (var element2 in list)
                {
                    if (element2.IdExerciseSet == element)
                    {
                        newEntry.Exercises.Add(new ExerciseModel()
                        {
                            IdExercise = element2.IdExercise,
                            ExerciseName = element2.ExerciseName,
                            IdExerciseSet = element2.IdExerciseSet,

                        });
                        newEntry.ExerciseSetTitle = element2.ExerciseSetTitle;
                    }

                }
                listOfExerciseSets.Add(newEntry);
            }
            ExerciseSetsList = listOfExerciseSets;
        }

        
    }
}
