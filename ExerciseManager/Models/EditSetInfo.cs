using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    public class EditSetInfo
    {
        public List<string> ExerciseIdsToDelete { get; } = [];
        public List<ExerciseModel> ExercisesToAdd { get; } = [];
        public List<ExerciseModel> ExercisesToEdit { get; } = [];
    }
}
