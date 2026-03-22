using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using ExerciseManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExerciseManager.ViewModels
{
    public class CurrentTrainingViewModel : BaseViewModel
    {
        private ObservableCollection<ExerciseSetModel> exerciseSetsList;
        public ObservableCollection<ExerciseSetModel> ExerciseSetsList
        {
            get { return exerciseSetsList; }
            set
            {
                exerciseSetsList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TreeViewItem> exerciseSetsTreeView;
        public ObservableCollection<TreeViewItem> ExerciseSetsTreeView
        {
            get { return exerciseSetsTreeView; }
            set
            {
                exerciseSetsTreeView = value; OnPropertyChanged();
            }
        }
        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand TreeViewDoubleClickCommand { get; set; }

        public CurrentTrainingViewModel(ViewMediator viewMediator) : base(viewMediator)
        {
            ExerciseSetsList = RetrieveSelectedSets();
            ExerciseSetsTreeView = ToTreeViewItemConverter.ConvertToTreeViewItemList(exerciseSetsList);
            TreeViewDoubleClickCommand = new RelayCommand(OnTreeViewDoubleClicked);
        }

        private ObservableCollection<ExerciseSetModel> RetrieveSelectedSets()
        {
            ObservableCollection<ExerciseSetModel> output = null;
            try
            {
                output = viewMediator.RetrieveData("SELECTED_SETS") as ObservableCollection<ExerciseSetModel>;

            }
            catch (ArgumentException e)
            {
                ErrorMessage = e.Message;
            }
            catch (KeyNotFoundException e)
            {
                ErrorMessage = e.Message;
            }

            if(output != null)
                viewMediator.DeleteData("SELECTED_SETS");

            return output;
        }

        public void OnTreeViewDoubleClicked(object sender)
        {
            
        }
    }
    }

