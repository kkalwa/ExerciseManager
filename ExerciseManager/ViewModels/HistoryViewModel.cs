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
    public class HistoryViewModel: BaseViewModel
    {
        public HistoryViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            DateSelectedCommand = new RelayCommand<DateTime>(onDateSelected);
            trainingsCollection = new ObservableCollection<TrainingHistoryModel>(trainingRepository.GetTrainingHistoryByUserId(viewMediator.CurrentUser.Id));
            foreach (var training in trainingsCollection)
            {
                DatabaseDates.Add(training.Date);
            }
        }

        private void onDateSelected(DateTime SelectedDate)
        {
            try
            {
                var selectedTraining = trainingsCollection.First(p => p.Date.Date == SelectedDate.Date);
                SelectedExercises = new ObservableCollection<ExerciseModel>(selectedTraining.Exercises);
            }
            catch(InvalidOperationException)
            {
                
            }
        }

        public ICommand DateSelectedCommand { get; set; }

        private ITrainingRepository trainingRepository = new TrainingRepository();

        /**
         * Properties associated with the view
         */
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                onDateSelected(value);
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TrainingHistoryModel> trainingsCollection;
        public ObservableCollection<TrainingHistoryModel> TrainingsCollection
        {
            get { return trainingsCollection; }
            set
            {
                trainingsCollection = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ExerciseModel> selectedExercises;
        public ObservableCollection<ExerciseModel> SelectedExercises
        {
            get { return selectedExercises; }
            set
            {
                selectedExercises = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DateTime> databaseDates=[];
        public ObservableCollection<DateTime> DatabaseDates
        {
            get
            {
                return databaseDates;
            }
            set
            {
                databaseDates = value;
                OnPropertyChanged();
            }
        }

    }
}
