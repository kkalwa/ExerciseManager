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

        public ICommand SaveTrainingCommand { get; set; }
        
        

        public ManageCurrentTrainingViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            trainingRepository = new TrainingRepository();
            ActualExercisesForTraining = RetrieveActualExercises();
            SaveTrainingCommand = new RelayCommand(SaveTraining);
        }

        private ITrainingRepository trainingRepository;
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

        private void SaveTraining(object parameter)
        {
            
            try 
            {
                trainingRepository.AddTraining(new TrainingModel(DateTime.Now, ActualExercisesForTraining));
            }catch(InvalidOperationException e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
