using System.Windows;

namespace ZzzLab.MicroServer.Views
{
    /// <summary>
    /// SplashScreenWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        public double Progress
        {
            get { return progressBar.Value; }
            set { progressBar.Value = value; }
        }

        public string LoadingText
        {
            get { return LoadingStatus.Text; }
            set { LoadingStatus.Text = value; }
        }

        public static string AppVersion => $"version {AppConstant.AppVersion}";

        public SplashScreenWindow()
        {
            this.DataContext = this;
            InitializeComponent();
        }
    }
}