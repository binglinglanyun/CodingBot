using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodingBot.Common
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeAction;

        private readonly Func<T, bool> _canExecuteAction;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecuteAction != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecuteAction != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public DelegateCommand(Action<T> executeAction)
            : this(executeAction, null)
        {
        }

        public DelegateCommand(Action<T> executeAction,
            Func<T, bool> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public bool CanExecute(object obj)
        {
            _parameter = obj;
            if (_canExecuteAction != null)
            {
                return _canExecuteAction(Parameter);
            }
            return true;
        }

        public virtual void Execute(object obj)
        {
            _parameter = obj;
            if (CanExecute(obj))
            {
                _executeAction(Parameter);
            }
        }

        protected object _parameter;
        protected T Parameter
        {
            get
            {
                if (_parameter is T)
                {
                    return (T)_parameter;
                }
                else
                {
                    return (T)Convert.ChangeType(_parameter, typeof(T));
                }
            }
        }
    }

    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action action)
            : base((o) => action())
        {
        }
    }
}
