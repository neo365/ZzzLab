using Microsoft.AspNetCore.Http;
using System.Text;
using ZzzLab.Net.Http;

namespace ZzzLab.Web.Logging
{
    public sealed class HttpRequestLog : IHttpLog
    {
        public HttpUrl? Url { set; get; }
        public string TraceIdentifier { set; get; } = Guid.NewGuid().ToString();
        public DateTime RegDateTime { get; }
        public string? Host => Url?.Host;
        public string? Scheme => Url?.Scheme;
        public string? Method { set; get; }
        public string? Protocol { set; get; }
        public string? Path => Url?.Path;
        public IEnumerable<KeyValuePair<string, string>>? Query => Url?.Query;
        public string? QueryString => Url?.QueryString;
        public IDictionary<string, string>? Headers { set; get; }
        public string? Body { get; set; }
        public HttpRequestLogContents? Contents { get; set; }
        public string? UserId { set; get; }
        public string? IpAddress { set; get; }
        public string? Referer => GetHeader("Referer");
        public string? UserAgent => GetHeader("User-Agent");
        public static string? MachineName => Environment.MachineName;

        public HttpRequestLog()
        {
            RegDateTime = DateTime.Now;
        }

        public HttpRequestLog SetTraceIdentifier(string id)
        {
            this.TraceIdentifier = id;

            return this;
        }

        public HttpRequestLog SetRequest(HttpRequest request)
        {
            Url = HttpUrl.Create($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
            Protocol = request.Protocol;
            Method = request.Method;
            UserId = request.GetUserId();
            IpAddress = request.GetIp();

            if (request.Headers != null && request.Headers.Any())
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();

                foreach (var (key, value) in request.Headers)
                {
                    dic.Add(key, value.ToString());
                }

                Headers = dic;
            }

            this.Contents = new HttpRequestLogContents();

            if (request.HasFormContentType)
            {
                if (request.Form != null)
                {
                    if (request.Form.Any())
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();

                        foreach (var formItem in request.Form)
                        {
                            dic.Add(formItem.Key, formItem.Value.ToString());
                        }
                        this.Contents.Form = dic;
                    }

                    if (request.Form.Files != null && request.Form.Files.Any())
                    {
                        List<HttpRequestLogFile> list = new List<HttpRequestLogFile>(request.Form.Files.Count);
                        foreach (var fileItem in request.Form.Files)
                        {
                            HttpRequestLogFile f = new HttpRequestLogFile
                            {
                                Name = fileItem.Name,
                                ContentType = fileItem.ContentType,
                                FileName = fileItem.FileName,
                                Length = fileItem.Length
                            };

                            list.Add(f);
                        }

                        this.Contents.Files = list;
                    }
                }
            }

            if (Contents.Hasfile == false)
            {
                this.Body = ReadBody(request.Body);

                if (request.HasFormContentType == false)
                {
                    this.Contents.Raw = this.Body;
                }
            }

            return this;
        }

        private static string ReadBody(Stream stream)
        {
            string body = string.Empty;

            using (StreamReader sr = new StreamReader(stream, leaveOpen: true))// 파이프라인이므로 스트림은 계속 열려있어야 한다.
            {
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
                if (stream.CanRead) body = sr.ReadToEndAsync().Result;
            }

            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);

            return body;
        }

        private string? GetHeader(string key)
        {
            if (string.IsNullOrWhiteSpace(key)
                || Headers == null
                || Headers.Any() == false
                || Headers.ContainsKey(key) == false)
            {
                return null;
            }

            return Headers[key];
        }

        public string GetHeaderString()
        {
            var builder = new StringBuilder();

            if (Headers != null && Headers.Any())
            {
                int count = Headers.Count;
                foreach (var header in Headers)
                {
                    count--;
                    builder.Append($"{header.Key}: {header.Value}");
                    if (count > 0) builder.Append(System.Environment.NewLine);
                }
                return builder.ToString();
            }

            return string.Empty;
        }

        private string? _cachedToString;

        public string ToString(bool forceRead)
        {
            if (_cachedToString == null || forceRead == true)
            {
                // Use 2kb as a rough average size for request headers
                var builder = new StringBuilder();

                builder.Append($"{Method} {Url} {Protocol}");
                builder.Append(System.Environment.NewLine);
                builder.Append(GetHeaderString());
                builder.Append(System.Environment.NewLine);
                builder.Append(System.Environment.NewLine);
                builder.Append(Body);

                _cachedToString = builder.ToString();
            }

            return _cachedToString;
        }

        public override string ToString()
            => ToString(false);
    }

    public class HttpRequestLogContents
    {
        public IDictionary<string, string>? Form { set; get; }
        public IEnumerable<HttpRequestLogFile>? Files { set; get; }
        public string? Raw { set; get; }

        public bool Hasfile => (Files != null && Files.Any());
    }

    public class HttpRequestLogFile
    {
        public string? Name { set; get; }
        public string? ContentType { set; get; }
        public string? FileName { set; get; }
        public long Length { set; get; } = 0L;
    }
}