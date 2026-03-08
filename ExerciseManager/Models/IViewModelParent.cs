using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    public interface IViewModelParent
    {
        public void ChangeChildViewModel(string nameOfNewViewModel); 
    }
}
