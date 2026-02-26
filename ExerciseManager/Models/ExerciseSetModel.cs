using System;
using System.Collections.Generic;
using System.Text;


namespace ExerciseManager.Models
{
    public class ExerciseSetModel
    {
        public string IdExerciseSet { get; set; }
        public string IdUser { get; set; }
        public string ExerciseSetTitle { get; set; }
        public List<ExerciseModel> Exercises { get; set; } = new();

        public override string ToString()
        {
            return $"IdExerciseSet: {IdExerciseSet}, IdUser: {IdUser}, ExerciseSetTitle: {ExerciseSetTitle}";
        }
    }
}
