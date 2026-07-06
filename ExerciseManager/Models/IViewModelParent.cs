using System;
using System.Collections.Generic;
using System.Text;

namespace ExerciseManager.Models
{
    public interface IViewModelParent
    {
        public void ChangeChildViewModel(string nameOfNewViewModel);
        public void StoreDataFromChildViewModel(string nameOfData, object data);
        public object RetrieveData(string nameOfData);
        public void DeleteData(string nameOfData);

    }
}
