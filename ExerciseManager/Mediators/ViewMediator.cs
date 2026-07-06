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
        public IViewModelParent ViewModelParent {  get; set; }
        public void ChangeViewTo(BaseViewModel newView)
        {
                       ViewChanged?.Invoke(newView);
        }
        public void StoreData(string nameOfData, object data)
        {
            ViewModelParent.StoreDataFromChildViewModel(nameOfData, data);
        }
        public object RetrieveData(string nameOfData)
        {
            return ViewModelParent.RetrieveData(nameOfData);
        }
        public void DeleteData(string nameOfData)
        {
            ViewModelParent.DeleteData(nameOfData);
        }

        public ViewMediator()
        { 
        }
    }
}
