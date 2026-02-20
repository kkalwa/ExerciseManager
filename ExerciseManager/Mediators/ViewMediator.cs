using ExerciseManager.Models;
using ExerciseManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Mediators
{
    public class ViewMediator
    {
        public UserModel CurrentUser { get; set; } = new UserModel();
        public event Action<BaseViewModel> ViewChanged;
        public void ChangeViewTo(BaseViewModel newView)
        {
                       ViewChanged?.Invoke(newView);
        }

        public ViewMediator()
        { 
        }
    }
}
