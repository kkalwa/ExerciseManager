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
    public class ManageExercisesViewModel: BaseViewModel
    {
        private List<ExerciseSetModel> listOfExerciseSets;
        public List<ExerciseSetModel> ListOfExerciseSets { get { return listOfExerciseSets; } 
            set
            {
                listOfExerciseSets = value;
                OnPropertyChanged();
            } 
        }

        private List<ManageExercisesModel> listOfUserSets;
        public List<ManageExercisesModel> ListOfUserSets
        {
            get { return listOfUserSets; }
            set
            {
                listOfUserSets = value;
                OnPropertyChanged();
            }
        }

        private List<ExerciseSetModel> outputList;

        public List<ExerciseSetModel> OutputList 
        {
            get { return outputList; }
            set { outputList = value; 
                OnPropertyChanged(); 
            }
        }

        public ICommand OpenAddNewSetCommand { get; set; }

        private IDBRepository DBRepository { get; set; }

        public ManageExercisesViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            DBRepository = new DBRepository();
            OpenAddNewSetCommand = new RelayCommand( (o) => { viewMediator.ViewModelParent.ChangeChildViewModel("CreateNewSetViewModel"); });
            List<ManageExercisesModel> list = DBRepository.GetExercisesBasedOnUserId(viewMediator.CurrentUser.Id);
            List<string> listOfSetIds = DBRepository.GetDistinctIdSetsForUser(viewMediator.CurrentUser.Id);
            List<ExerciseSetModel> listOfExerciseSets = new();

            foreach (var element in listOfSetIds)
            {
                ExerciseSetModel newEntry = new() { IdExerciseSet = element };

                foreach(var element2 in list)
                {
                    if(element2.IdExerciseSet == element)
                    {
                        newEntry.Exercises.Add(new ExerciseModel()
                        {
                            IdExercise = element2.IdExercise,
                            ExerciseName = element2.ExerciseName,
                            IdExerciseSet = element2.IdExerciseSet,
                            
                        });
                        newEntry.ExerciseSetTitle = element2.ExerciseSetTitle;
                    }

                }
                listOfExerciseSets.Add(newEntry);
            }

            OutputList = listOfExerciseSets;
        }
    }
}
