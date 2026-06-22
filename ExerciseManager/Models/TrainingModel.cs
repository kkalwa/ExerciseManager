using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExerciseManager.Models
{
    public class TrainingModel
    {
        public ObservableCollection<ExerciseModel> ExerciseList { get; set; }
        public DateOnly Date { get; set; } = new DateOnly();
        public TrainingModel(ObservableCollection<ExerciseModel> exerciseList) 
        { 
            ExerciseList = exerciseList;
        }
    }
}
