using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    internal interface IDBRepository
    {
        public ExerciseSetModel GetExerciseSetById(string id);
        public List<ExerciseSetModel> GetExerciseSetByUserId(string id);
        public List<ManageExercisesModel> GetExercisesByExerciseSetId(string IdExerciseSet);
        public List<ManageExercisesModel> GetExercisesBasedOnUserId(string Id);
        public List<string> GetDistinctIdSetsForUser(string id_user);
        public void InsertNewExerciseSet(ExerciseSetModel newSet);
        
    }
}
