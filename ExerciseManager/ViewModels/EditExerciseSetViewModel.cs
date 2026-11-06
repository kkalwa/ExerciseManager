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
        private string? sqlError;

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
        public ICommand SaveChangesCommand { get; set; }
        public string? SqlError
        {
            get { return sqlError; }
            set
            {
                sqlError = value;
                OnPropertyChanged();
            }
        }

        private void AddExercise(ExerciseModel model)
        {
            ExerciseModel newEntry = new ExerciseModel()
            {
                IdExerciseSet = ExerciseSetToEdit.IdExerciseSet ?? throw new InvalidOperationException("Invalid exercise set ID"),
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
            exerciseSetToEdit = (ExerciseSetModel)viewMediator.RetrieveData(TRANSFER_KEY);
            RemoveExerciseCommand = new RelayCommand<ExerciseModel>(RemoveExercise);
            AddExerciseToListCommand = new RelayCommand<ExerciseModel>(AddExercise);
            SaveChangesCommand = new RelayCommand<object>(SaveChanges);
            editSetInfo.IdExerciseSet = exerciseSetToEdit.IdExerciseSet ?? throw new InvalidOperationException("Invalid exercise set ID");

        }

        private void SaveChanges(object obj)
        {
            SqlError = null;
            editSetInfo.ExerciseSetName = exerciseSetToEdit.ExerciseSetTitle ?? throw new InvalidOperationException("Invalid exercise set name");
            foreach (ExerciseModel exercise in ExerciseSetToEdit.Exercises)
            {
                if(exercise.IdExercise != string.Empty)
                editSetInfo.ExercisesToUpdate.Add(exercise);
            }
            try
            {
                DBRepository.UpdateExerciseSet(editSetInfo);
            }
            catch (Exception ex)
            {
                SqlError = $"Error updating exercise set: {ex.Message}";
            }
        }
    }
}
