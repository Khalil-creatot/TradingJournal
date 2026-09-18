using System.Windows.Input;

namespace TradingJournal.wpf.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;


        //  Constructor that initializes the command with an action to execute and an optional function to determine if the command can execute.
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }


        // This event is raised when the command's ability to execute changes.
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        // This method checks if the command can be executed with the provided parameter.
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }


        // This method executes the command's action with the provided parameter.
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }



    }
}
