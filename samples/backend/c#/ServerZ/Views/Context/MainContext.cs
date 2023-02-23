using System.ComponentModel;
using ZzzLab.Event;
using ZzzLab.MicroServer.Views;

namespace ZzzLab.MicroServer.Context
{
    internal partial class MainContext : WindowContextBase
    {
        private new MainWindow Owner { set; get; }

        public string? TargetName { set; get; }

        public string? AppVersion { set; get; }

        public ServerStatus ServerStatus { set; get; }

        public bool IsServerStoped { set; get; } = false;

        public bool IsServerStarted { set; get; } = true;

        public MainContext(MainWindow owner) : base(owner)
        {
            this.BaseWindowTitle = AppConstant.DisplayName;
            this.Owner = owner;
            this.PropertyChanged += MainContext_PropertyChanged;
        }

        public static bool Shutdown()
            => true;

        private void MainContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Do Nothing
        }
    }
}