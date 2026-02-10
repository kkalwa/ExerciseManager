using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.ViewModels
{
    
    public class LoginViewModel: BaseViewModel
    {
        public RelayCommand GoToUserPanelCommand { get; set; }

        public LoginViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            GoToUserPanelCommand = new RelayCommand(GoToUserPanel);
        }

        private void GoToUserPanel(object obj)
        {
            viewMediator.ChangeViewTo(new UserPanelViewModel(viewMediator));
        }
    }
}
