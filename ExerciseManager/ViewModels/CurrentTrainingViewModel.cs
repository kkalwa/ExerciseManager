using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    public class CurrentTrainingViewModel : BaseViewModel
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
        private ObservableCollection<ExerciseSetModel> actualExercisesForTraining = [];
        public ObservableCollection<ExerciseSetModel> ActualExercisesForTraining
        {
            get { return actualExercisesForTraining; }
            set
            {
                actualExercisesForTraining = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TreeViewItem> exerciseSetsTreeView;
        public ObservableCollection<TreeViewItem> ExerciseSetsTreeView
        {
            get { return exerciseSetsTreeView; }
            set
            {
                exerciseSetsTreeView = value; 
                OnPropertyChanged();
            }
        }
        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }
        private ExerciseSetModel selectedExerciseSet;
        public ExerciseSetModel SelectedExerciseSet
        {
            get { return selectedExerciseSet; }
            set
            {
                selectedExerciseSet = value;
                OnPropertyChanged();
            }
        }

        private ExerciseModel selectedExercise;
        public ExerciseModel SelectedExercise
        {
            get { return selectedExercise; }
            set
            {
                selectedExercise = value;
                OnPropertyChanged();
            }
        }
        public ICommand TreeViewDoubleClickCommand { get; set; }
        public ICommand TreeViewDoubleClickExCommand { get; set;}
        public ICommand ConfirmCommand { get; set; }

        public CurrentTrainingViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            ExerciseSetsList = RetrieveSelectedSets();
            ExerciseSetsTreeView = ToTreeViewItemConverter.ConvertToTreeViewItemList(exerciseSetsList);
            TreeViewDoubleClickCommand = new RelayCommand(OnTreeViewDoubleClicked);
            TreeViewDoubleClickExCommand = new RelayCommand(OnTreeViewDoubleClickEx);
            ConfirmCommand = new RelayCommand(OnConfirmClicked);
        }

        private void OnConfirmClicked(object obj)
        {
            viewMediator.StoreData("ACTUAL_EXERCISES", ActualExercisesForTraining);
            viewMediator.ViewModelParent.ChangeChildViewModel("ManageCurrentTrainingViewModel");
        }

        private void OnTreeViewDoubleClickEx(object obj)
        {
            if (obj is ExerciseModel)
            {
                //removeExerciseFromList(obj as ExerciseModel);
                int index = findSetIndexByExercise(obj as ExerciseModel);
                ActualExercisesForTraining[index].Exercises.Remove(obj as ExerciseModel);
                if (ActualExercisesForTraining[index].Exercises.Count == 0)
                {
                    ActualExercisesForTraining.RemoveAt(index);
                }
            }
            else if (obj is ExerciseSetModel)
            {
                removeEntireSetFromList(obj as ExerciseSetModel);
            }
        }

        private int findSetIndexByExercise(ExerciseModel exerciseModel)
        {
            for(int i = 0; i < ActualExercisesForTraining.Count; i++)
            {
                if(ActualExercisesForTraining[i].Exercises.Contains(exerciseModel))
                {
                    return i;
                }
            }
            return -1;
        }

        private void removeEntireSetFromList(ExerciseSetModel exerciseSetModel)
        {
            foreach(var exerciseSet in ActualExercisesForTraining)
            {
                if(exerciseSet == exerciseSetModel)
                {
                    ActualExercisesForTraining.Remove(exerciseSet);
                    
                    break;
                }
            }
        }

        private void removeExerciseFromList(ExerciseModel exerciseModel)
        {
            foreach(var exerciseSet in ActualExercisesForTraining)
            {
                foreach(var exercise in exerciseSet.Exercises)
                {
                    if(exercise == exerciseModel)
                    {
                        exerciseSet.Exercises.Remove(exercise);
                        break;
                    }
                }
            }
        }

        private ObservableCollection<ExerciseSetModel> RetrieveSelectedSets()
        {
            ObservableCollection<ExerciseSetModel> output = null;
            try
            {
                output = viewMediator.RetrieveData("SELECTED_SETS") as ObservableCollection<ExerciseSetModel>;

            }
            catch (ArgumentException e)
            {
                ErrorMessage = e.Message;
            }
            catch (KeyNotFoundException e)
            {
                ErrorMessage = e.Message;
            }

            if(output != null)
                viewMediator.DeleteData("SELECTED_SETS");

            return output;
        }

        public void OnTreeViewDoubleClicked(object sender)
        {
            if(isSelectedSetAlreadyInCollection())
            {
                appendExerciseToExistingSet();
            }else
            {
                addExerciseSetToCollection();
                appendExerciseToExistingSet();
            }   
        }

        private bool isSelectedSetAlreadyInCollection()
        {
            foreach (var exerciseSet in ActualExercisesForTraining)
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
    }

