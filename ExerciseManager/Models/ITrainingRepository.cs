using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExerciseManager.Models
{
    public interface ITrainingRepository
    {
        public void AddTraining(TrainingModel trainingModel);
        public ObservableCollection<TrainingModel> GetTrainingsByUserId(string userId);
    }
}
