using ZzzLab.Logging;
using ZzzLab.Web.Logging;

namespace ZzzLab.AspCore.Logging
{
    /// <summary>
    /// Log 저장을 위한 Queue 처리 Commond
    /// </summary>
    public class HttpLoggerCommand : HttpLoggerCommandBase, IHttpLoggerCommand
    {
        private object? _Data = null;

        public HttpLoggerCommand() : base(new NullLogger())
        {
        }

        public override void SetRequest(HttpRequestLog req)
            => this._Data = req;

        public override void SetResponse(HttpResponseLog res)
            => this._Data = res;

        public override bool LogWriter()
        {
            if (_Data == null) return false;
#if false
            try
            {
                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    string sql;
                    QueryParameterCollection Parameters = QueryParameterCollection.Create();

                    if (this.Data is HttpRequestLog req)
                    {
                        sql = DB.GetQuery("HTTP_LOGGER", "REQUEST");

                        Parameters.Add("traking_id", req.TraceIdentifier);
                        Parameters.Add("user_id", req.UserId);
                        Parameters.Add("ip_addr", req.IpAddress);
                        Parameters.Add("user_agent", req.UserAgent);
                        Parameters.Add("method", req.Method);
                        Parameters.Add("path", req.Path);
                        Parameters.Add("query_string", req.QueryString);
                        Parameters.Add("protocol", req.Protocol);
                        Parameters.Add("request", JsonConvert.SerializeObject(req));
                        Parameters.Add("date_request", req.RegDateTime);
                        Parameters.Add("execute_time", "0");
                        Parameters.Add("machine_name", HttpRequestLog.MachineName);
                    }
                    else if (this.Data is HttpResponseLog res)
                    {
                        sql = DB.GetQuery("HTTP_LOGGER", "RESPONSE");

                        Parameters.Add("traking_id", res.TraceIdentifier);
                        Parameters.Add("status_code", res.StatusCode);
                        Parameters.Add("response_header", res.GetHeaderString());
                        Parameters.Add("response_body", res.Body);
                        Parameters.Add("date_response", res.RegDateTime);
                        Parameters.Add("execute_time", res.ExecuteTime);
                        Parameters.Add("machine_name", HttpResponseLog.MachineName);
                    }
                    else return false;

                    return (DB.Excute(Query.Create(sql, Parameters, 10)) <= 0);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
#endif

            return false;
        }
    }
}