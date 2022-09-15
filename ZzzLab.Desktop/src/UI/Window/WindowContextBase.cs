using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace System.ComponentModel
{
    public abstract class WindowContextBase : INotifyPropertyChanged
    {
        public virtual bool Result { protected set; get; } = false;

        protected virtual Window Owner { set; get; }

        protected string BaseWindowTitle = string.Empty;

        protected string _WindowTitle = string.Empty;

        public string WindowTitle
        {
            get { return _WindowTitle; }
            set
            {
                if (_WindowTitle == value) return;

                _WindowTitle = BaseWindowTitle + value;

                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        public WindowContextBase(Window owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.Owner.StateChanged += Owner_StateChanged;
            this.Owner.Activated += Owner_Activated;

            DefaultCommands();

            InitializeCommand();
        }

        #region WPF Error?

        public WindowState WindowState
        { get { return this.Owner.WindowState; } }

        public bool IsActive
        { get { return this.Owner.IsActive; } }

        public event EventHandler? Activated;

        private void Owner_Activated(object? sender, EventArgs e)
        {
            Activated?.Invoke(sender, e);
        }

        public event EventHandler? StateChanged;

        private void Owner_StateChanged(object? sender, EventArgs e)
        {
            StateChanged?.Invoke(sender, e);
        }

        #endregion WPF Error?

        #region Command

        protected abstract void InitializeCommand();

        public ICommand? CancelCommand { protected set; get; }

        protected virtual void DefaultCommands()
        {
            CancelCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    this.WindowClose(false);
                },
                CanExecuteDelegate = param => true
            };
        }

        #endregion Command

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region Helper

        public void WindowClose(bool result = true)
        {
            if (Owner != null)
            {
                this.Owner.Dispatcher.Invoke(() =>
                {
                    this.Result = result;
                    Owner.DialogResult = result;
                    Owner.Close();
                });
            }
        }

        #endregion Helper
    }
}