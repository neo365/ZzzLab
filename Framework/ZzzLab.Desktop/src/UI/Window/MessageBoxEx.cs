namespace System.Windows
{
    public static class MessageBoxEx
    {
        private static Window CreateWindow()
        {
            return new Window() { WindowState = WindowState.Minimized, Topmost = true };
        }

#if WIN32_SUPPORT

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

#endif

        public static MessageBoxResult Show(Window? owner, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None)
        {
            MessageBoxResult result = MessageBoxResult.None;

            if (owner != null)
            {
                MessageBox.Show(owner, message, title, button, icon);
            }
            else
            {
                Window dummy = CreateWindow();

#if WIN32_SUPPORT
                SetForegroundWindow(dummy.Handle);
#endif
                dummy.Activate();
                result = MessageBox.Show(dummy, message, title, button, icon);
                dummy.Close();
            }

            return result;
        }

        public static void Warning(string message, string title = "경고", Window? owner = null)
        {
            MessageBoxEx.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void Error(string message, string title = "에러", Window? owner = null)
        {
            MessageBoxEx.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Information(string message, string title = "알림", Window? owner = null)
        {
            MessageBoxEx.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool YesNo(string message, string title = "선택", Window? owner = null)
        {
            return (MessageBoxEx.Show(owner, message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
        }

        public static bool OkCancel(string message, string title = "선택", Window? owner = null)
        {
            return (MessageBoxEx.Show(owner, message, title, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK);
        }
    }
}