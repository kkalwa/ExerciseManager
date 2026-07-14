using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExerciseManager.ViewModels
{
    public class HistoryViewModel: BaseViewModel
    {
        public HistoryViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            TrainingsCollection = trainingRepository.GetTrainingsByUserId(viewMediator.CurrentUser.Id);
        }

        private ITrainingRepository trainingRepository = new TrainingRepository();
        
        /**
         * Properties associated with the view
         */
        private ObservableCollection<ExerciseSetModel> trainingsCollection;
        public ObservableCollection<ExerciseSetModel> TrainingsCollection
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
