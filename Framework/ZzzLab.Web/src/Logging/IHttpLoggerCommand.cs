using ZzzLab.Helper.Execute;

namespace ZzzLab.Web.Logging
{
    public interface IHttpLoggerCommand : IMessageQueueCommand
    {
        void SetRequest(HttpRequestLog req);

        void SetResponse(HttpResponseLog res);

        bool LogWriter();
    }
}