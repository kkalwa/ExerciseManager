using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    public class TrainingHistoryModel
    {
        public int IdTraining { get; set; }
        public DateTime Date { get; set; }
        public int IdUser { get; set; }
        public List<ExerciseModel> Exercises { get; set; } = [];

    }
}
