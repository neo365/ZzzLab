using Microsoft.AspNetCore.Http;
using System.Dynamic;
using System.Web;

namespace ZzzLab.Web
{
    public static partial class RequestExtension
    {
        /// <summary>
        /// Query string에서 값을 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string? GetQueryString(this HttpRequest request, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            return request.Query.ContainsKey(key) ? request.Query[key].ToString() : null;
        }

        /// <summary>
        /// Form에서 값을 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string? GetFormParameter(this HttpRequest request, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            return request.HasFormContentType && request.Form.ContainsKey(key) ? request.Form[key].ToString() : null;
        }

        /// <summary>
        /// Request 데이터를 가져온다. 우선적으로 QueryString -> Form 순으로 리턴한다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <param name="key"></param>
        /// <returns></returns>

        public static string? GetRequest(this HttpRequest request, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;

            if (request.Query.ContainsKey(key)) return request.Query[key].ToString();
            if (request.HasFormContentType && request.Form.ContainsKey(key)) return request.Form[key].ToString();

            return null;
        }

        /// <summary>
        /// Querystring과 FormValue 를 dynamic 데이터로 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <returns></returns>
        public static dynamic GetRequest(this HttpRequest request)
            => GetParameters(request).Aggregate(new ExpandoObject() as IDictionary<string, object?>,
                            (a, p) => { a.Add(p); return a; });

        /// <summary>
        /// Querystring과 FormValue 를 dynamic 데이터로 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <returns></returns>
        public static IDictionary<string, object?> GetParameters(this HttpRequest request)
        {
            IDictionary<string, object?> parameters = new Dictionary<string, object?>();

            IQueryCollection queryData = request.Query;

            foreach (string key in queryData.Keys)
            {
                if (parameters.ContainsKey(key)) continue;

                parameters.Add(key, request.Query[key]);
            }

            if (request.HasFormContentType)
            {
                IFormCollection formData = request.Form;

                foreach (string key in formData.Keys)
                {
                    if (parameters.ContainsKey(key)) continue;

                    parameters.Add(key, request.Form[key]);
                }
            }

            return parameters;
        }

        /// <summary>
        /// 쿠키값을 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string? GetCookie(this HttpRequest request, string name)
        {
            if (request == null) return null;
            if (string.IsNullOrWhiteSpace(name)) return null;

            // 한글 깨지는 문제 수정
            return request.Cookies.ContainsKey(name) ? HttpUtility.UrlDecode(request.Cookies[name]?.ToString()) : null;
        }

        /// <summary>
        /// 헤더 값을 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string? GetHeader(this HttpRequest request, string name)
            => string.IsNullOrWhiteSpace(name) == false && request.Headers.ContainsKey(name) ? request.Headers[name].ToString() : null;

        /// <summary>
        /// User-Agent 값을 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <returns></returns>
        public static string? GetUserAgent(this HttpRequest request)
            => RequestExtension.GetHeader(request, "User-Agent");

        /// <summary>
        /// Referer값을 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <returns></returns>
        public static string? GetReferer(this HttpRequest request)
            => RequestExtension.GetHeader(request, "Referer");

        /// <summary>
        /// 업로드 파일데이터를 Byte Array형태로 가져온다.
        /// </summary>
        /// <param name="file">Microsoft.AspNetCore.Http.IFormFile</param>
        /// <returns></returns>
        public static byte[] ReadFully(this IFormFile file)
        {
            using (Stream input = file.OpenReadStream())
            {
                byte[] buffer = new byte[input.Length];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// 접속 IP를 가져온다.
        /// </summary>
        /// <param name="request">Microsoft.AspNetCore.Http.HttpRequest</param>
        /// <returns></returns>
        public static string? GetIp(this HttpRequest request)
            => request?.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}