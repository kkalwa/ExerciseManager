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
        public ICommand TreeViewDoubleClickCommand { get; set; }
        public ICommand TreeViewDoubleClickExCommand { get; set; }


        public ManageTrainingsViewModel(ViewMediator viewMediator) : base(viewMediator) 
        {
            DBRepository = new DBRepository();
            AddToTrainingCommand = new RelayCommand<object>(AddToTraining);
            ConfirmTrainingCommand =  new RelayCommand<object>(ConfirmTraining);
            TreeViewDoubleClickCommand = new RelayCommand<object>(OnTreeViewDoubleClicked);
            TreeViewDoubleClickExCommand = new RelayCommand<object>(OnTreeViewDoubleClickEx);
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

        public void OnTreeViewDoubleClicked(object sender)
        {
            if (isSelectedSetAlreadyInCollection())
            {
                appendExerciseToExistingSet();
            }
            else
            {
                addExerciseSetToCollection();
                appendExerciseToExistingSet();
            }
        }

        private bool isSelectedSetAlreadyInCollection()
        {
            foreach (var exerciseSet in SelectedExerciseSetsList)
            {
                if (exerciseSet.ExerciseSetTitle == SelectedExerciseSet.ExerciseSetTitle)
                    return true;
            }

            return false;
        }

        private void appendExerciseToExistingSet()
        {
            foreach (var exerciseSet in ActualExercisesForTraining)
            {
                if (exerciseSet.ExerciseSetTitle == SelectedExerciseSet.ExerciseSetTitle)
                {
                    exerciseSet.Exercises.Add(SelectedExercise);
                    break;
                }
            }
        }

        private void addExerciseSetToCollection()
        {
            ActualExercisesForTraining.Add(new ExerciseSetModel()
            {
                IdExerciseSet = SelectedExerciseSet.IdExerciseSet,
                IdUser = SelectedExerciseSet.IdUser,
                ExerciseSetTitle = SelectedExerciseSet.ExerciseSetTitle
            });
        }
    }


        private void OnTreeViewDoubleClickEx(object obj)
        {
            if (obj is ExerciseModel)
            {
                //removeExerciseFromList(obj as ExerciseModel);
                int index = findSetIndexByExercise(obj as ExerciseModel);
                ExerciseSetsList[index].Exercises.Remove(obj as ExerciseModel);
                if (ExerciseSetsList[index].Exercises.Count == 0)
                {
                    ExerciseSetsList.RemoveAt(index);
                }
            }
            else if (obj is ExerciseSetModel)
            {
                removeEntireSetFromList(obj as ExerciseSetModel);
            }
        }

        private int findSetIndexByExercise(ExerciseModel exerciseModel)
        {
            for (int i = 0; i < ExerciseSetsList.Count; i++)
            {
                if (ExerciseSetsList[i].Exercises.Contains(exerciseModel))
                {
                    return i;
                }
            }
            return -1;
        }

        private void removeEntireSetFromList(ExerciseSetModel exerciseSetModel)
        {
            foreach (var exerciseSet in ExerciseSetsList)
            {
                if (exerciseSet == exerciseSetModel)
                {
                    ExerciseSetsList.Remove(exerciseSet);

                    break;
                }
            }
        }

        private void PrepareExerciseSetsList()
        {
            List<ManageExercisesModel> list = DBRepository.GetExercisesBasedOnUserId(viewMediator.CurrentUser.Id);
            List<string> listOfSetIds = DBRepository.GetDistinctIdSetsForUser(viewMediator.CurrentUser.Id);
            ObservableCollection<ExerciseSetModel> listOfExerciseSets = new();

            foreach (var element in listOfSetIds)
            {
                ExerciseSetModel newEntry = new() { IdExerciseSet = element, IdUser = viewMediator.CurrentUser.Id };

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
