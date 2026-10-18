using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using ExerciseManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    public class ManageTrainingsViewModel: BaseViewModel
    {
        private ObservableCollection<ExerciseSetModel> exerciseSetsList = [];
        public ObservableCollection<ExerciseSetModel> ExerciseSetsList
        {
            get { return exerciseSetsList; }
            set
            {
                exerciseSetsList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ExerciseSetModel> selectedExerciseSetsList = [];
        public ObservableCollection<ExerciseSetModel> SelectedExerciseSetsList
        {
            get { return selectedExerciseSetsList; }
            set
            {
                selectedExerciseSetsList = value;
                OnPropertyChanged();
            }
        }

        private ExerciseSetModel selectedItem;
        public ExerciseSetModel SelectedItem
            {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

       public string SelectedExercisesVisible
        {
            get
            {
                if (SelectedExerciseSetsList.Count > 0)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
        }
        public DBRepository DBRepository { get; private set; }

        public ICommand AddToTrainingCommand { get; set; }
        public ICommand ConfirmTrainingCommand { get; set; }
        public ICommand TreeViewDoubleClickCommand { get; set; }
        public ICommand TreeViewDoubleClickExCommand { get; set; }


        public ManageTrainingsViewModel(ViewMediator viewMediator) : base(viewMediator) 
        {
            DBRepository = new DBRepository();
            ConfirmTrainingCommand =  new RelayCommand<object>(ConfirmTraining);
            TreeViewDoubleClickCommand = new RelayCommand<object>(OnTreeViewDoubleClicked);
            TreeViewDoubleClickExCommand = new RelayCommand<object>(OnTreeViewDoubleClickEx);
            PrepareExerciseSetsList();
        }
        
        private void ConfirmTraining(object obj)
        {
            viewMediator.StoreData("ACTUAL_EXERCISES", SelectedExerciseSetsList);
            viewMediator.ViewModelParent.ChangeChildViewModel("ManageCurrentTrainingViewModel");
        }

        public void OnTreeViewDoubleClicked(object sender)
        {
            if (sender is ExerciseSetModel selectedExerciseSet)
            {
                //Should I do anything in this case?
            }else if (sender is ExerciseModel selectedExercise)
            {
                if (isSetOfExerciseAlreadyInSelectedSets(selectedExercise))
                {
                    appendExerciseToAppropriateSetInSelectedSets(selectedExercise);
                }
                else
                {
                    addExerciseSetToSelectedSets(selectedExercise);
                    appendExerciseToAppropriateSetInSelectedSets(selectedExercise);
                }
            }
        }

        private void addExerciseSetToSelectedSets(ExerciseModel selectedExercise)
        {
            SelectedExerciseSetsList.Add(new ExerciseSetModel()
            {
                IdExerciseSet = selectedExercise.IdExerciseSet,
                IdUser = viewMediator.CurrentUser.Id,
                ExerciseSetTitle = exerciseSetsList[findSetIndexByExercise(selectedExercise)].ExerciseSetTitle
            });
        }

        private void appendExerciseToAppropriateSetInSelectedSets(ExerciseModel selectedExercise)
        {
            foreach(ExerciseSetModel currentSet in SelectedExerciseSetsList)
            {
                if(currentSet.IdExerciseSet == selectedExercise.IdExerciseSet)
                {
                    currentSet.Exercises.Add(selectedExercise);
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedExercisesVisible));
        }

        private bool isSetOfExerciseAlreadyInSelectedSets(ExerciseModel selectedExercise)
        {
            foreach (ExerciseSetModel currentSet in SelectedExerciseSetsList)
            {
                if (currentSet.IdExerciseSet == selectedExercise.IdExerciseSet)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnTreeViewDoubleClickEx(object obj)
        {
            if (obj is ExerciseModel)
            {
                //removeExerciseFromList(obj as ExerciseModel);
                int index = findSetIndexByExercise(obj as ExerciseModel);
                SelectedExerciseSetsList[index].Exercises.Remove(obj as ExerciseModel);
                if (SelectedExerciseSetsList[index].Exercises.Count == 0)
                {
                    SelectedExerciseSetsList.RemoveAt(index);
                    OnPropertyChanged(nameof(SelectedExercisesVisible));
                }
            }
            else if (obj is ExerciseSetModel)
            {
                removeEntireSetFromList(obj as ExerciseSetModel);
            }
        }

        private int findSetIndexByExercise(ExerciseModel exerciseModel)
        {
            for (int i = 0; i < ExerciseSetsList.Count; i++)
            {
                if (ExerciseSetsList[i].Exercises.Contains(exerciseModel))
                {
                    return i;
                }
            }
            return -1;
        }

        private void removeEntireSetFromList(ExerciseSetModel exerciseSetModel)
        {
            foreach (var exerciseSet in SelectedExerciseSetsList)
            {
                if (exerciseSet == exerciseSetModel)
                {
                    SelectedExerciseSetsList.Remove(exerciseSet);
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedExercisesVisible));
        }

        private void PrepareExerciseSetsList()
        {
            List<ManageExercisesModel> list = DBRepository.GetExercisesBasedOnUserId(viewMediator.CurrentUser.Id);
            List<string> distinctIdSetNumbers = list.
                Select(e => e.IdExerciseSet).
                Distinct().
                ToList();

            foreach(string idSet in distinctIdSetNumbers)
            {
                List<ExerciseModel> exercisesInSet = list.
                                        Where(e => e.IdExerciseSet == idSet).
                    Select(e => new ExerciseModel()
                    {
                        IdExercise = e.IdExercise,
                        ExerciseName = e.ExerciseName,
                        IdExerciseSet = e.IdExerciseSet,
                        Description = e.Description,
                        ExerciseWeights = new ObservableCollection<MutableStringValue>(e.ExerciseWeights)
                    }).
                    ToList();
                
                ExerciseSetsList.Add(new ExerciseSetModel()
                {
                    IdExerciseSet = idSet,
                    IdUser = viewMediator.CurrentUser.Id,
                    ExerciseSetTitle = list.Where(e => e.IdExerciseSet == idSet).Select(e => e.ExerciseSetTitle).FirstOrDefault(),
                    Exercises = new ObservableCollection<ExerciseModel>(exercisesInSet)
                });
            }
            /*List<string> listOfSetIds = DBRepository.GetDistinctIdSetsForUser(viewMediator.CurrentUser.Id);
            ObservableCollection<ExerciseSetModel> listOfExerciseSets = new();

            foreach (var element in listOfSetIds)
            {
                ExerciseSetModel newEntry = new() { IdExerciseSet = element, IdUser = viewMediator.CurrentUser.Id };

                foreach (var element2 in list)
                {
                    if (element2.IdExerciseSet == element)
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
            ExerciseSetsList = listOfExerciseSets;*/
        }

        
    }
}
