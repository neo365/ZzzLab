using System.Windows;
using System.Windows.Input;

namespace ZzzLab.MicroServer.Views
{
    public partial class MainWindow
    {
        private void InitializeUI()
        {
            this.Title = "ZzzLab ZzzLab.MicroServer™";
            //this.WindowState = WindowState.Normal;
            //this.ResizeMode = ResizeMode.NoResize;

            this.notifyIcon.DataContext = this.DataContext;
            this.notifyIcon.ContextMenu.DataContext = this.DataContext;
        }

        private void ActivateWindow()
        {
            this.Topmost = true;
            this.Activate();
            this.Topmost = false;
        }

        internal void HideWindow()
        {
            this.Topmost = false;
            this.WindowState = WindowState.Normal;
            this.Hide();

            if (isLoaded)
            {
                ShowBalloonTipText($"여전히 서비스가 구동 중입니다. 서비스 종료는 시스템 트레이에서 종료를 선택해 주세요.");
            }
        }

        internal void ShowWindow()
        {
            this.WindowState = WindowState.Normal;
            this.Show();
            this.ActivateWindow();
        }

        #region Position Chage

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            switch (this.WindowState)
            {
                // 사이즈 키우기 안되게
                case WindowState.Maximized:
                    this.WindowState = WindowState.Normal;
                    break;

                case WindowState.Normal:
                    Topmost = true;
                    ShowInTaskbar = true;
                    ShowActivated = true;
                    break;

                case WindowState.Minimized:
                    Topmost = false;
                    ShowInTaskbar = false;
                    ShowActivated = false;
                    break;
            }

            Topmost = false; ;
        }

        #endregion Position Chage

        #region Input Event

        // 칭이동 기능구현
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;

                    // 3 or any where you want to set window location after
                    // return from maximum state
                    Application.Current.MainWindow.Top = 3;
                }

                this.DragMove();
            }
        }

        #endregion Input Event
    }
}