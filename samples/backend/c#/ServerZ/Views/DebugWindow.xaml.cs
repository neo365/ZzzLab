using System.ComponentModel;
using System.Windows;

namespace ZzzLab.MicroServer.Views
{
    /// <summary>
    /// DebugWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DebugWindow : Window
    {
        private bool IsClosing = false;

        public DebugWindow()
        {
            InitializeComponent();
            this.Closing += WindowClosing;
        }

        private void WindowClosing(object? sender, CancelEventArgs e)
        {
            if (IsClosing == false)
            {
                this.Hide();
                e.Cancel = true;

                return;
            }

            base.OnClosed(e);
        }

        private void BtnClearClick(object? sender, RoutedEventArgs e)
        {
            logText.Clear();
        }

        private void BtnCloseClick(object? sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        //private bool AutoScroll = true;

        //private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        //{
        //    // User scroll event : set or unset auto-scroll mode
        //    if (e.ExtentHeightChange == 0)
        //    {   // Content unchanged : user scroll event
        //        if (ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight)
        //        {   // Scroll bar is in bottom
        //            // Set auto-scroll mode
        //            AutoScroll = true;
        //        }
        //        else
        //        {   // Scroll bar isn't in bottom
        //            // Unset auto-scroll mode
        //            AutoScroll = false;
        //        }
        //    }

        //    // Content scroll event : auto-scroll eventually
        //    if (AutoScroll && e.ExtentHeightChange != 0)
        //    {   // Content changed and auto-scroll mode set
        //        // Autoscroll
        //        ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ExtentHeight);
        //    }
        //}

        public void WindowExit()
        {
            IsClosing = true;
            this.Close();
        }
    }
}