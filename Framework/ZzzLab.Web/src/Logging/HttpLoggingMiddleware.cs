using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using ZzzLab.Diagnostics;
using ZzzLab.ExceptionEx;
using ZzzLab.Helper.Execute;
using ZzzLab.Net.Http;
using ZzzLab.Web.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ZzzLab.Web.Logging
{
    public sealed class HttpLoggingMiddleware<T> where T : IHttpLoggerCommand
    {
        private static MessageQueue LoggerQueue { get; } = new MessageQueue();

        public readonly RequestDelegate _next;

        public HttpLoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public Task Invoke(HttpContext context)
            => InvokeInternal(context);

        private async Task InvokeInternal(HttpContext context)
        {
            HttpRequest request = context.Request;

            request.EnableBuffering(); // Request 재사용 설정

            context.TraceIdentifier = Guid.NewGuid().ToString(); // 키가 같은 경우가 가끔생김

            HttpRequestLog requestLog = new HttpRequestLog().SetTraceIdentifier(context.TraceIdentifier)
                                                            .SetRequest(request);

            if (Activator.CreateInstance(typeof(T)) is IHttpLoggerCommand requestCommand)
            {
                requestCommand.SetRequest(requestLog);
                LoggerQueue.Enqueue(requestCommand);
            }

            var response = context.Response;
            // Store the original body stream for restoring the response body back to its original stream
            var originalResponseBodyStream = response.Body;

            // Create new memory stream for reading the response; Response body streams are write-only, therefore memory stream is needed here to read
            await using var responseMs = new MemoryStream();
            response.Body = responseMs;

            var stropWatch = Stopwatch.StartNew();
            await _next(context);

            TimeSpan executeTime = stropWatch.GetElapsedTime();

            var responseLog = new HttpResponseLog
            {
                TraceIdentifier = context.TraceIdentifier,
                Protocol = request.Protocol,
                StatusCode = response.StatusCode,
                Headers = GetHeaders(response.Headers),
                ExecuteTime = executeTime.TotalMilliseconds
            };

            // Set stream pointer position to 0 before reading
            if (responseMs.CanSeek) responseMs.Seek(0, SeekOrigin.Begin);

            // Read the body from the stream
            responseLog.Body = await new StreamReader(responseMs).ReadToEndAsync();

            // Reset the position to 0 after reading
            responseMs.Seek(0, SeekOrigin.Begin);

            // 이 작업을 마지막으로 수행하면 최종 결과가 응답으로 표시되도록 할 수 있습니다.
            // (이 결과 응답은 리디렉션된 경로 또는 미들웨어와 관련된 리디렉션 / 재실행이 있는 경우 다른 특수 경로에서 올 수 있습니다.)
            // ASP.NET은 메모리 스트림의 내용을 표시하는 것을 좋아하지 않는 것 같습니다.
            // 따라서 ASP.NET Core 엔진에서 제공하는 원래 스트림을 다시 스왑해야 합니다.
            // 그런 다음 이전 메모리 스트림에서 이 원래 스트림으로 다시 씁니다.
            // (내용은 이 시점에서 메모리 스트림에 기록됩니다.ASP.NET 엔진이 메모리 스트림의 내용을 표시하는 것을 거부할 뿐입니다.)
            response.Body = originalResponseBodyStream;

            if (responseMs.Length > 0 && response.StatusCode == StatusCodes.Status400BadRequest)
            {
                // 일단 기본에러랑 동일한지 보고 아니면 그냥 쓴다.
                try
                {
                    AspCoreDefaultError? defaultError = JsonConvert.DeserializeObject<AspCoreDefaultError>(responseLog.Body);
                    if (defaultError != null)
                    {
                        defaultError.TraceId = context.TraceIdentifier;
                        responseLog.Body = JsonConvert.SerializeObject(defaultError.ToRestResult());

                        byte[] bytes = Encoding.Default.GetBytes(responseLog.Body);
                        response.ContentLength = bytes.Length;
                        await response.Body.WriteAsync(bytes);
                    }
                    else await response.Body.WriteAsync(responseMs.ToArray());
                }
                catch (Exception ex)
                {
                    Logger.Warning(ex);
                    await response.Body.WriteAsync(responseMs.ToArray());
                }
            }
            else if (responseMs.Length == 0 && response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                context.Response.ContentType = Text.Plain;
                responseLog.Body = "서버에러가 발생하였습니다.";

                var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (error != null)
                {
                    context.Response.ContentType = Application.Json;
                    var exception = error.InnerException ?? error;

                    RestServerErrorResult res = new RestServerErrorResult()
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ErrorMessage = exception.GetAllMessages(),
                        Error = exception.GetAllExceptionInfo(),
                        ErrorDescription = StatusCodes.Status500InternalServerError.ToStatusMessage(),
                    };

                    responseLog.Body = res.ToString();
                }
                try
                {
                    byte[] bytes = Encoding.Default.GetBytes(responseLog.Body);
                    response.ContentLength = bytes.Length;
                    await response.Body.WriteAsync(bytes);
                }
                catch (Exception ex)
                {
                    Logger.Warning(ex);
                    await response.Body.WriteAsync(responseMs.ToArray());
                }
            }
            else await response.Body.WriteAsync(responseMs.ToArray());

            if (Activator.CreateInstance(typeof(T)) is IHttpLoggerCommand responseCommand)
            {
                responseCommand.SetResponse(responseLog);
                LoggerQueue.Enqueue(responseCommand);
            }
        }

        private static IDictionary<string, string> GetHeaders(IHeaderDictionary headers)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var (key, value) in headers)
            {
                dic.Add(key, value.ToString());
            }

            return dic;
        }

#if false // 사용안함 막자
        //private async Task<string> ReadRequestBodyAync(Stream stream)
        //{
        //    string body = string.Empty;

        //    using (StreamReader reader = new StreamReader(stream, leaveOpen: true))// 파이프라인이므로 스트림은 계속 열려있어야 한다.
        //    {
        //        if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
        //        body = await reader.ReadToEndAsync();
        //    }

        //    if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);

        //    return body;
        //}
#endif


    }
}