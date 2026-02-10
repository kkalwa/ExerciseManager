using ExerciseManager.Commands;
using ExerciseManager.Mediators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.ViewModels
{
    public class UserPanelViewModel: BaseViewModel
    {
        public RelayCommand GoToLoginCommand { get; set; }
        public UserPanelViewModel(ViewMediator viewMediator): base(viewMediator)
        {
            GoToLoginCommand = new RelayCommand(GoToLogin);
        }

        private void GoToLogin(object obj)
        {
            viewMediator.ChangeViewTo(new LoginViewModel(viewMediator));
        }
    }
}
