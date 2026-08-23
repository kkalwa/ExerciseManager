using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExerciseManager.Models
{
    public interface ITrainingRepository
    {
        public void AddTraining(TrainingModel trainingModel);
        public ObservableCollection<TrainingHistoryModel> GetTrainingsByUserId(string userId);
        public List<TrainingHistoryModel> GetTrainingHistoryByUserId(string userId);
    }
}
