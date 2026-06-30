using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExerciseManager.Models
{
    public class TrainingModel
    {
        public ObservableCollection<ExerciseSetModel> ExerciseList { get;  }
        public DateTime Date { get;  } 
        public string IdUser { get=>ExerciseList.First().IdUser; }
    
        public TrainingModel(DateTime Date,ObservableCollection<ExerciseSetModel> ExerciseList) 
        { 
            this.Date = Date;
            this.ExerciseList = ExerciseList;
        }
    }
}
