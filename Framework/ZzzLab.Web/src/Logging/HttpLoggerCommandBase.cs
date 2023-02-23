using ZzzLab.Logging;

namespace ZzzLab.Web.Logging
{
    public abstract class HttpLoggerCommandBase : IHttpLoggerCommand
    {
        public virtual IZLogger Logger { get; } = new NullLogger();

        public HttpLoggerCommandBase(IZLogger? logger = null)
        {
            if (logger != null) Logger = logger;
        }

        public void Execute()
        {
            try
            {
                LogWriter();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public abstract void SetRequest(HttpRequestLog req);

        public abstract void SetResponse(HttpResponseLog res);

        public abstract bool LogWriter();
    }
}