using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    public class CreateNewSetViewModel : BaseViewModel
    {
        private ObservableCollection<ExerciseModel> exercises = new();
        public ObservableCollection<ExerciseModel> Exercises
        {
            get { return exercises; }
            set
            {
                exercises = value;
                OnPropertyChanged();
            }
        }
        private string exerciseSetTitle;
        public string ExerciseSetTitle
        {
            get { return exerciseSetTitle; }
            set
            {
                exerciseSetTitle = value;
                OnPropertyChanged();
            }
        }

        private string sqlResultText = string.Empty;
        public string SQLResultText
        {
            get { return sqlResultText; }
            set
            {
                sqlResultText = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddExerciseCommand { get; set; }
        public ICommand AddNewLineCommand { get; set; }
        public ICommand RemoveLineCommand { get; set; }
        private IDBRepository Repository = new DBRepository();

        public CreateNewSetViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            Exercises.Add(new ExerciseModel());

            AddExerciseCommand = new RelayCommand(AddExercise);
            AddNewLineCommand = new RelayCommand(AddNewLine);
            RemoveLineCommand = new RelayCommand(RemoveLine);
        }
        private void AddExercise(object sender)
        {
            ExerciseSetModel newExerciseSet = new()
            {
                IdUser = viewMediator.CurrentUser.Id,
                ExerciseSetTitle = this.ExerciseSetTitle,
                Exercises = new List<ExerciseModel>(this.Exercises)
            };

            if (IsDataValid(newExerciseSet))
            {
                FillEmptyWeightsWithZeros(newExerciseSet);
                SQLResults result = Repository.InsertNewExerciseSet(newExerciseSet);

                if (result == SQLResults.Success)
                {
                    SQLResultText = "Sukces";
                    viewMediator.ViewModelParent.ChangeChildViewModel("ManageExercisesViewModel");
                }
                else
                {
                    SQLResultText = "Nie udało się zapisać danych do bazy";
                }
            }

        }

        private void AddNewLine(object sender)
        {
            Exercises.Add(new ExerciseModel());
        }

        private void RemoveLine(object sender)
        {
            if (Exercises.Count > 1)
            {
                Exercises.RemoveAt(Exercises.Count - 1);
            }
        }

        private bool IsDataValid(ExerciseSetModel ExerciseSet)
        {
            bool output = true;

            if (String.IsNullOrEmpty(ExerciseSet.ExerciseSetTitle))
            {
                output = false;
            }
            else
            {
                foreach (var Element in ExerciseSet.Exercises)
                {
                    if (String.IsNullOrEmpty(Element.ExerciseName))
                    {
                        output = false;
                        break;
                    }
                }
            }

            return output;
        }

        private void FillEmptyWeightsWithZeros(ExerciseSetModel ExerciseSet)
        {
            foreach (var Element in ExerciseSet.Exercises)
            {
                for (int i = 0; i < Element.ExerciseWeights.Count; i++)
                {
                    if (String.IsNullOrEmpty(Element.ExerciseWeights[i].Value))
                    {
                        Element.ExerciseWeights[i].Value = "0";
                    }
                }
            }
        }
    }
}
