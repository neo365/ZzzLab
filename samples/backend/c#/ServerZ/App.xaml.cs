using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZzzLab.MicroServer.Views;
using ZzzLab.Web;
using Forms = System.Windows.Forms;

namespace ZzzLab.MicroServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SplashScreenWindow? SplashWindow;
        public static MainWindow? BaseWindow { private set; get; }

        internal static Task? WebTask { get; private set; }

        public App() : base()
        {
            Logger.Info($"{AppConstant.AppName} Start: GUI Mode");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //initialize the splash screen and set it as the application main window
            SplashWindow = new SplashScreenWindow();
            this.MainWindow = SplashWindow;
            SplashWindow.Show();

            Task.Factory.StartNew(() =>
            {
                if (InitializeApp())
                {
                    Thread.Sleep(50);
                    this.Dispatcher.Invoke(() =>
                    {
                        this.MainWindow = App.BaseWindow = new MainWindow();
                        this.MainWindow.Show();
                        SplashWindow.Close();
                        SplashWindow = null;
                    });
                }
                else
                {
                    Forms.MessageBoxEx.Error("시스템 초기화에 실패 하였습니다.");
                    Environment.Exit(-1);
                }
            });
        }

        private void SetSplashScreen(string text, double progress)
        {
            if (SplashWindow == null) return;
            SplashWindow.Dispatcher.Invoke(() => SplashWindow.LoadingText = text);
            SplashWindow.Dispatcher.Invoke(() => SplashWindow.Progress = progress);
            Thread.Sleep(100);
        }

        private bool InitializeApp()
        {
            try
            {
                SetSplashScreen("프로그램 시작중", 1);

                Process[] processList = Process.GetProcessesByName("LabStdI");

                if (processList != null && processList.Length > 1)
                {
                    Forms.MessageBoxEx.Error("LabStd™가 이미 실행중입니다. 프로그램을 종료합니다.");
                    Environment.Exit(-1);

                    Process.GetCurrentProcess().Kill();
                    App.Current.Shutdown();
                }

                SetSplashScreen("웹 서비스 구동중", 60);

                WebBuilder.StartServer();

                SetSplashScreen("구동중", 100);

                return true;
            }
            catch (Exception ex)
            {
                Forms.MessageBoxEx.Error(ex.Message);
            }

            return false;
        }
    }
}