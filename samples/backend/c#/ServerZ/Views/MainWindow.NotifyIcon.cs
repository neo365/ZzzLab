using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Forms = System.Windows.Forms;

namespace ZzzLab.MicroServer.Views
{
    public partial class MainWindow
    {
        public void ShowBalloonTipText(string message, int timeout = 1500)
        {
            try
            {
                Dispatcher.Invoke((Action)delegate ()
                {
                    this.notifyIcon.BalloonTipTitle = AppConstant.DisplayName;
                    this.notifyIcon.BalloonTipText = message;

                    if (string.IsNullOrEmpty(this.notifyIcon.BalloonTipText?.Trim()) == false)
                    {
                        this.notifyIcon.ShowBalloonTip(timeout);
                    }
                });
            }
            catch (Exception ex)
            {
                Forms.MessageBoxEx.Error(ex.Message);
            }
        }

        private void OnNotifyIconClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is NotifyIcon)
            {
                // Do Nothing
            }
        }

        private void OnNotifyIconDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is NotifyIcon)
            {
                try
                {
                    int webPort = Configurator.Get("WebPort").ToInt();
                    string url = $"http://localhost:{webPort}";
                    System.Diagnostics.Process.Start("explorer", url);
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Error(ex.Message);
                }
            }
        }
    }
}