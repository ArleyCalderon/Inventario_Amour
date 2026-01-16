using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventario_Amour.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        private readonly Action<object> _parameterizedExecute;
        private readonly Func<object, bool> _parameterizedCanExecute;

        public event EventHandler CanExecuteChanged;

        // Constructor para comandos sin parámetros
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // Constructor para comandos con parámetros
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _parameterizedExecute = execute;
            _parameterizedCanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null) return _canExecute();
            if (_parameterizedCanExecute != null) return _parameterizedCanExecute(parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
                _execute();
            else if (_parameterizedExecute != null)
                _parameterizedExecute(parameter);
        }

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
