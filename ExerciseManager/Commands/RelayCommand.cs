using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ExerciseManager.Commands
{
    public class RelayCommand<T> : ICommand
    {
        readonly Action<T> execute;
        readonly Predicate<T> canExecute;

        public RelayCommand(Action<T> execute) :
            this(execute, null)
        {

        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
