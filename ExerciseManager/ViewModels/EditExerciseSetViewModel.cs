using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    public class EditExerciseSetViewModel : BaseViewModel
    {
        private ExerciseSetModel exerciseSetToEdit;
        private EditSetInfo editSetInfo = new();
        private IDBRepository DBRepository { get; } = new DBRepository();

        public ExerciseSetModel ExerciseSetToEdit
        {
            get { return exerciseSetToEdit; }
            set
            {
                exerciseSetToEdit = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveExerciseCommand { get; set; }
        public ICommand AddExerciseToListCommand { get; set; }

        private void AddExercise(ExerciseModel model)
        {
            ExerciseModel newEntry = new ExerciseModel()
            {
                IdExerciseSet = ExerciseSetToEdit.IdExerciseSet
            };
            ExerciseSetToEdit.Exercises.Add(newEntry);
            editSetInfo.ExercisesToAdd.Add(newEntry);
        }

        private void RemoveExercise(ExerciseModel model)
        {
            if (editSetInfo.ExercisesToAdd.Contains(model))
            {
                editSetInfo.ExercisesToAdd.Remove(model);
            }
            else
            {
                editSetInfo.ExerciseIdsToDelete.Add(model.IdExercise);
            }
            ExerciseSetToEdit.Exercises.Remove(model);
        }

        public static string TRANSFER_KEY { get; } = "ExerciseSetToEdit";
        public EditExerciseSetViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            ExerciseSetToEdit = (ExerciseSetModel)viewMediator.RetrieveData(TRANSFER_KEY);
            RemoveExerciseCommand = new RelayCommand<ExerciseModel>(RemoveExercise);
            AddExerciseToListCommand = new RelayCommand<ExerciseModel>(AddExercise);
        }
    }
}
