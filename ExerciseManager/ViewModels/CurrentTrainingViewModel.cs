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

        public CurrentTrainingViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            ExerciseSetsList = RetrieveSelectedSets();
            ExerciseSetsTreeView = ToTreeViewItemConverter.ConvertToTreeViewItemList(exerciseSetsList);
            TreeViewDoubleClickCommand = new RelayCommand(OnTreeViewDoubleClicked);
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
            if(selectedSetIsAlreadyInCollection())
            {
                appendExerciseToExistingSet();
            }else
            {
                addExerciseSetToCollection();
                appendExerciseToExistingSet();
            }
            
        }

        private bool selectedSetIsAlreadyInCollection()
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

