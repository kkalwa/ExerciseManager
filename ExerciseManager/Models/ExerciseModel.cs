using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    public class ExerciseModel
    {
        private static readonly int maxWeights = 5;
        public string IdExercise { get; set; }
        public string IdExerciseSet { get; set; }
        public string ExerciseName { get; set; }
        public string[] ExerciseWeights { get; set; } = new string[maxWeights];
    }
}
