using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    public interface ITrainingRepository
    {
        public void AddTraining(TrainingModel trainingModel);
    }
}
