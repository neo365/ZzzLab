using System.Net;

namespace ZzzLab.Net.Http
{
    public static class HttpExtension
    {
        public static string ToStatusMessage(this int statusCode)
        {
            switch (statusCode)
            {
                case 0: return "Not connect. Verify Network.";
                case 400: return "잘못된 요청입니다. (Bad Request)";
                case 401: return "페이지 접근권한이 없습니다. (Unauthorized) ";
                case 402: return "이 요청은 결제가 필요합니다. (Payment Required) ";
                case 403: return "사용자가 리소스에 대한 필요 권한을 갖고 있지 않습니다. (Forbidden)";
                case 404: return "서버가 요청한 페이지를 찾을 수 없습니다. (Not Found)";
                case 405: return "요청이 잘못되었습니다. (Method Not Allowed)";
                case 408: return "요청시간이 초과 되었습니다. (Request Timeout)";
                case 500: return "서버에 오류가 발생하여 요청을 수행할 수 없습니다. (Internal server error.)";
                case 501: return "서버에 요청을 수행할 수 있는 기능이 없습니다. (Internal server error.)";
                case 503: return "서버가 오버로드되었거나 유지관리를 위해 다운되었기 때문에 현재 서버를 사용할 수 없습니다. (Service unavailable.)";
                default: return $"[{statusCode}]알수 없는 에러";
            };
        }

        public static string ToStatusMessage(this HttpStatusCode statusCode)
            => ToStatusMessage((int)statusCode);

        private static readonly string[][] HttpReasonPhrases = new string[][]
        {
            null,

            new string[]
            {
                /* 100 */ "Continue",
                /* 101 */ "Switching Protocols",
                /* 102 */ "Processing"
            },

            new string[]
            {
                /* 200 */ "OK",
                /* 201 */ "Created",
                /* 202 */ "Accepted",
                /* 203 */ "Non-Authoritative Information",
                /* 204 */ "No Content",
                /* 205 */ "Reset Content",
                /* 206 */ "Partial Content",
                /* 207 */ "Multi-Status"
            },

            new string[]
            {
                /* 300 */ "Multiple Choices",
                /* 301 */ "Moved Permanently",
                /* 302 */ "Found",
                /* 303 */ "See Other",
                /* 304 */ "Not Modified",
                /* 305 */ "Use Proxy",
                /* 306 */ null,
                /* 307 */ "Temporary Redirect"
            },

            new string[]
            {
                /* 400 */ "Bad Request",
                /* 401 */ "Unauthorized",
                /* 402 */ "Payment Required",
                /* 403 */ "Forbidden",
                /* 404 */ "Not Found",
                /* 405 */ "Method Not Allowed",
                /* 406 */ "Not Acceptable",
                /* 407 */ "Proxy Authentication Required",
                /* 408 */ "Request Timeout",
                /* 409 */ "Conflict",
                /* 410 */ "Gone",
                /* 411 */ "Length Required",
                /* 412 */ "Precondition Failed",
                /* 413 */ "Request Entity Too Large",
                /* 414 */ "Request-Uri Too Long",
                /* 415 */ "Unsupported Media Type",
                /* 416 */ "Requested Range Not Satisfiable",
                /* 417 */ "Expectation Failed",
                /* 418 */ null,
                /* 419 */ null,
                /* 420 */ null,
                /* 421 */ null,
                /* 422 */ "Unprocessable Entity",
                /* 423 */ "Locked",
                /* 424 */ "Failed Dependency",
                /* 425 */ null,
                /* 426 */ "Upgrade Required", // RFC 2817
            },

            new string[]
            {
                /* 500 */ "Internal Server Error",
                /* 501 */ "Not Implemented",
                /* 502 */ "Bad Gateway",
                /* 503 */ "Service Unavailable",
                /* 504 */ "Gateway Timeout",
                /* 505 */ "Http Version Not Supported",
                /* 506 */ null,
                /* 507 */ "Insufficient Storage"
            }
        };

        public static string ToStatusText(int code)
        {
            if (code >= 100 && code < 600)
            {
                int i = code / 100;
                int j = code % 100;
                if (j < HttpReasonPhrases[i].Length)
                {
                    return HttpReasonPhrases[i][j];
                }
            }
            return null;
        }
    }
}