using ExerciseManager.Mediators;
using ExerciseManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExerciseManager.ViewModels
{
    public class ManageCurrentTrainingViewModel : BaseViewModel
    {
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

        public ManageCurrentTrainingViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            ActualExercisesForTraining = RetrieveActualExercises();
        }

        private ObservableCollection<ExerciseSetModel> RetrieveActualExercises()
        {
            ObservableCollection<ExerciseSetModel> output = null;
            try
            {
                output = viewMediator.RetrieveData("ACTUAL_EXERCISES") as ObservableCollection<ExerciseSetModel>;

            }
            catch (ArgumentException e)
            {
                ErrorMessage = e.Message;
            }
            catch (KeyNotFoundException e)
            {
                ErrorMessage = e.Message;
            }

            if (output != null)
                viewMediator.DeleteData("ACTUAL_EXERCISES"); 

            return output;
        }
    }
}
