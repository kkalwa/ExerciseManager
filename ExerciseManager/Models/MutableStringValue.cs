using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExerciseManager.Models
{
    public class MutableStringValue: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public static MutableStringValue Empty => new MutableStringValue(string.Empty);
        public string Value
        {
            get { return field; }
            set { field = value; OnPropertyChanged(); }
        }

        public MutableStringValue(string value)
        {
            Value = value;
        }
        
    }
}
