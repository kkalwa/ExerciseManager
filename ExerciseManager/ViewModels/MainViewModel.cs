using ExerciseManager.Mediators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.ViewModels
{
    
    internal class MainViewModel: BaseViewModel
    {
        private BaseViewModel currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            CurrentViewModel = new LoginViewModel(viewMediator);
            viewMediator.ViewChanged += (newView) => CurrentViewModel = newView;
        }

        
    }
}
