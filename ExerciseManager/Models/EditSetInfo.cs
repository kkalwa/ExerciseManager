using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    public class EditSetInfo
    {
        public List<string> ExerciseIdsToDelete { get; } = [];
        public List<ExerciseModel> ExercisesToAdd { get; } = [];
        public List<ExerciseModel> ExercisesToUpdate { get; } = [];
        public string? IdExerciseSet { get; set; } 
        public string? ExerciseSetName { get; set; }

        public bool HasChanges()
        {
            return ExerciseIdsToDelete.Count > 0 || ExercisesToAdd.Count > 0 || ExercisesToUpdate.Count > 0;
        }
    }
}
