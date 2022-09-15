using Microsoft.AspNetCore.Http;
using System.Text;

namespace ZzzLab.Web.Logging
{
    public sealed class HttpFilterMiddleware
    {
        public readonly RequestDelegate _next;

        public HttpFilterMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public Task Invoke(HttpContext context)
            => InvokeInternal(context);

        private async Task InvokeInternal(HttpContext context)
        {
            HttpRequest request = context.Request;

            request.EnableBuffering(); // Request 재사용 설정

            var response = context.Response;
            // Store the original body stream for restoring the response body back to its original stream
            var originalResponseBodyStream = response.Body;

            // Create new memory stream for reading the response; Response body streams are write-only, therefore memory stream is needed here to read
            await using var responseMs = new MemoryStream();
            response.Body = responseMs;
            await _next(context);

            // Set stream pointer position to 0 before reading
            if (responseMs.CanSeek) responseMs.Seek(0, SeekOrigin.Begin);

            response.Body = originalResponseBodyStream;

            if (responseMs.Length > 0)
            {
                if (response.StatusCode == 500)
                {
                    string msg = "서버에러가 발생하였습니다.";
                    byte[] bytes = Encoding.Default.GetBytes(msg);
                    response.ContentLength = bytes.Length;
                    await response.Body.WriteAsync(bytes);
                }
            }
        }
    }
}