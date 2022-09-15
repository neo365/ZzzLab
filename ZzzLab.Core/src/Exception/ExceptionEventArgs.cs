namespace System
{
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

    public class ExceptionEventArgs : EventArgs
    {
        public string Message { set; get; }

        public Exception Ex { set; get; }

        public ExceptionEventArgs(string message) : this(message, null)
        {
        }

        public ExceptionEventArgs(Exception ex) : this(ex.Message, ex)
        {
        }

        public ExceptionEventArgs(string message, Exception ex) : base()
        {
            this.Message = message;
            this.Ex = ex;
        }
    }
}