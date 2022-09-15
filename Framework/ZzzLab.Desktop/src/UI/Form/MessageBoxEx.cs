namespace System.Windows.Forms
{
    public static class MessageBoxEx
    {
        private static Form CreateForm()
        {
            return new Form() { WindowState = FormWindowState.Maximized, TopMost = true };
        }

#if WIN32_SUPPORT

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

#endif

        public static DialogResult Show(IWin32Window? owner, string message, string title = "", MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            DialogResult result = DialogResult.None;

            if (owner != null)
            {
                MessageBox.Show(owner, message, title, button, icon);
            }
            else
            {
                using (Form dummy = CreateForm())
                {
#if WIN32_SUPPORT
                    SetForegroundWindow(dummy.Handle);
#endif
                    dummy.Activate();
                    result = MessageBox.Show(dummy, message, title, button, icon);
                    dummy.Close();
                    dummy.Dispose();
                }
            }

            return result;
        }

        public static void Warning(string message, string title = "경고", IWin32Window? owner = null)
        {
            MessageBoxEx.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string message, string title = "에러", IWin32Window? owner = null)
        {
            MessageBoxEx.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Information(string message, string title = "알림", IWin32Window? owner = null)
        {
            MessageBoxEx.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool YesNo(string message, string title = "선택", IWin32Window? owner = null)
        {
            return (MessageBoxEx.Show(owner, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        public static bool OkCancel(string message, string title = "선택", IWin32Window? owner = null)
        {
            return (MessageBoxEx.Show(owner, message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK);
        }

        public static bool RetryCancel(string message, string title = "선택", IWin32Window? owner = null)
        {
            return (MessageBoxEx.Show(owner, message, title, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry);
        }
    }
}