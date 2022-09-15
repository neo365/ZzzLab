namespace ZzzLab.UI.Controls
{
    public delegate void ClosingEventHandler(object sender, EventArgs e);

    public class WebBrowserEx : WebBrowser
    {
        // Define constants from winuser.h
        private const int WM_PARENTNOTIFY = 0x210;

        private const int WM_DESTROY = 2;

        public event ClosingEventHandler? Closing;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PARENTNOTIFY:
                    if (!DesignMode)
                    {
                        if (m.WParam.ToInt32() == WM_DESTROY)
                        {
                            Closing?.Invoke(this, EventArgs.Empty);
                        }
                    }
                    DefWndProc(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}