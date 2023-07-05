namespace System.Windows.Input
{
    public class CustomCommand : ICommand
    {
        public Predicate<object>? CanExecuteDelegate { get; set; }
        public Action<object>? ExecuteDelegate { get; set; }

        public CustomCommand(Predicate<object>? canExecuteDelegate = null)
        {
            if (canExecuteDelegate != null)
            {
                CanExecuteDelegate = canExecuteDelegate;
            }
        }

        #region ICommand Members

        public bool CanExecute(object? parameter)
        {
            if (CanExecuteDelegate != null && parameter != null)
            {
                return CanExecuteDelegate(parameter);
            }
            return true;// if there is no can execute default to true
        }

        private event EventHandler? CanExecuteChangedInternal;

        public void OnCanExecuteChanged()
        {
            EventHandler? handler = this.CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public void Execute(object? parameter)
        {
            if (ExecuteDelegate != null && parameter != null)
            {
                this.ExecuteDelegate(parameter);
            }
        }

        #endregion ICommand Members

        public void Destroy()
        {
            this.CanExecuteDelegate = _ => false;
            this.ExecuteDelegate = _ => { return; };
        }
    }
}