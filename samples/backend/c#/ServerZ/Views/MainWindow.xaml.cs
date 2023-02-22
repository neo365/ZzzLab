using System;
using System.IO;
using System.Windows;
using ZzzLab.MicroServer.Context;

namespace ZzzLab.MicroServer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainContext Context { get; }

        public MainWindow()
        {
            this.DataContext = this.Context = new MainContext(this);
            this.Context.PropertyChanged += WindowPropertyChanged;

            InitializeComponent();

            InitializeUI();
            InitializeEvent();
            InitializeDebugger();

            InitAgreement();

            //using (HtmlToImage htmlToImage = HtmlToImage.Create())
            //{
            //    if (htmlToImage.TryFromUrl("http://naver.com", out byte[] urlBytes, out _))
            //    {
            //        File.WriteAllBytes(@"capUrl.png", urlBytes);

            //        using (MemoryStream ms = new MemoryStream(urlBytes))
            //        {
            //            using (Bitmap bitmap = (Bitmap)Bitmap.FromStream(ms))
            //            {
            //                bitmap.Save(@"capUrl01.png", ImageFormat.Png);
            //                bitmap.Save(@"capUrl01.jpg", ImageFormat.Jpeg);
            //            }
            //        }
            //    }

            //    if (htmlToImage.TryFromHtml("<p>htmltest</p>", out byte[] htmlBytes, out _))
            //    {
            //        File.WriteAllBytes(@"caphtml.png", htmlBytes);
            //        using (MemoryStream ms = new MemoryStream(htmlBytes))
            //        {
            //            using (Bitmap bitmap = (Bitmap)Bitmap.FromStream(ms))
            //            {
            //                bitmap.Save(@"caphtml01.png", ImageFormat.Png);
            //                bitmap.Save(@"caphtml01.jpg", ImageFormat.Jpeg);
            //            }
            //        }
            //    }
            //
            //    GC.Collect();
            //}
        }

        private void InitAgreement()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Agreement.info");
            if (File.Exists(filePath))
            {
                this.Agreement.Text = File.ReadAllText(filePath);
            }
            else
            {
                this.Agreement.Text = "이용약관을 찾을 수 없습니다.";
            }
        }
    }
}