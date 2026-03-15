using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExerciseManager.Models
{
    public class ExerciseModel: INotifyPropertyChanged
    {
        private static readonly int maxWeights = 5;
        public string IdExercise { get; set; } = string.Empty;
        public string IdExerciseSet { get; set; } = string.Empty;
        public string ExerciseName { get; set; } = string.Empty;
        public ObservableCollection<MutableStringValue> exerciseWeights = new();
        public ObservableCollection<MutableStringValue> ExerciseWeights
        {
            get { return exerciseWeights; }
            set { exerciseWeights = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ExerciseModel() 
        {
                for (int i = 0; i < maxWeights; i++)
                {
                    ExerciseWeights.Add(new MutableStringValue("10"));
            }
        }
    }
}
