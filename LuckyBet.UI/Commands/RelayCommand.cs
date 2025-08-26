using System;
using System.Windows.Input;

namespace LuckyBet.UI.Commands
{
    public class RelayCommand : ICommand
    {
        // --- Action and Predicate types are now nullable ---
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute; // This whole predicate can be null

        // --- Constructor accepts nullable predicate ---
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // --- 'parameter' is now nullable object? ---
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        // --- 'parameter' is now nullable object? ---
        public void Execute(object? parameter) => _execute(parameter);

        // --- EventHandler is now nullable ---
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}