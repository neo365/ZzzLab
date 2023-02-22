using System;
using System.ComponentModel;
using System.Windows;
using ZzzLab.MicroServer.Context;
using Forms = System.Windows.Forms;

namespace ZzzLab.MicroServer.Views
{
    public partial class MainWindow
    {
        public void InitializeEvent()
        {
            this.Loaded += WindowLoaded;
            this.Closing += WindowClosing;
            this.Closed += WindowClosed;
        }

        #region Initialize

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.HideWindow();
            }
            base.OnStateChanged(e);
        }

        #endregion Initialize

        private bool isLoaded = false;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            this.HideWindow();

            ShowBalloonTipText($"서비스가 시작되었습니다.\n\r서비스 접속은 http://localhost:{AppConstant.WebPort} 에서 확인 가능합니다.");

            isLoaded = true;
        }

        private void WindowPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if ((sender is MainContext) == false) return;
            if (IsClosing) return;
        }

        #region Closing Support

        public bool IsClosing = false;

        public void WindowExit()
        {
            this.IsClosing = true;
            this.Close();
        }

        private void WindowClosing(object? sender, CancelEventArgs e)
        {
            if (IsClosing == false)
            {
                //this.HideWindow();
                e.Cancel = true;

                return;
            }

            this.Shutdown();
            base.OnClosed(e);
        }

        private void WindowClosed(object? sender, EventArgs e)
        {
            this.Shutdown();
        }

        public bool IsShutdown = false;

        private void Shutdown()
        {
            if (IsShutdown) return;

            if (this._Logs != null)
            {
                this._Logs.WindowExit();
                //this._Logs = null;
            }

            MainContext.Shutdown();

            //// 하다하다 종료안되면 쓰자.
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            //App.Current.Shutdown();

            IsShutdown = true;
        }

        #endregion Closing Support

        private void HyperlinkNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                string url = e.Uri.ToString();
                System.Diagnostics.Process.Start("explorer", url);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Error(ex.Message);
            }
        }

        private void WindowErrorReport(object sender, ExceptionEventArgs e)
        {
            Forms.MessageBoxEx.Error(e.Message);
        }
    }
}