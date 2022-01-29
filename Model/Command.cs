using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pedometer.Model
{
    public class Command : ICommand
    {
        // The action that is invoked when the command is activated.
        protected Action action = null;
        protected Action<object> parameterizedAction = null;

        // Enables the command execution.
        private bool canExecute = false;

        public bool CanExecute
        {
            get { return canExecute; }
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler Executing;
        public event EventHandler Executed;

        public Command(Action action, bool canExecute = true)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        // Defines if the command can be executed in the current state.
        bool ICommand.CanExecute(object parameter)
        {
            return canExecute;
        }

        // The command execution.
        void ICommand.Execute(object parameter)
        {
            Executing?.Invoke(this, EventArgs.Empty);

            Action action = this.action;
            Action<object> parameterizedAction = this.parameterizedAction;
            if (action != null)
            {
                action();
            }
            else
            {
                parameterizedAction?.Invoke(parameter);
            }

            Executed?.Invoke(this, EventArgs.Empty);
        }
    }
}
