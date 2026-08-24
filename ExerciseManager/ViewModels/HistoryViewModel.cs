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

        }

        private void onDateSelected(DateTime SelectedDate)
        {
            throw new NotImplementedException();
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


    }
}
