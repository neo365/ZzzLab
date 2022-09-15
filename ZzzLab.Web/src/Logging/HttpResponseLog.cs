using System.Text;
using ZzzLab.Net.Http;

namespace ZzzLab.Web.Logging
{
    public sealed class HttpResponseLog : IHttpLog
    {
        public string? TraceIdentifier { set; get; }
        public DateTime RegDateTime { get; } = DateTime.Now;
        public string? Protocol { set; get; }
        public int StatusCode { set; get; } = 0;
        public string? StatusText => HttpExtension.ToStatusText(StatusCode);
        public IDictionary<string, string> Headers { set; get; } = new Dictionary<string, string>();
        public string? Body { set; get; }
        public double ExecuteTime { set; get; } = 0D;
        public static string MachineName => Environment.MachineName;

        public HttpResponseLog()
        {
            RegDateTime = DateTime.Now;
        }

        public string? GetHeaderString()
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

            return null;
        }

        private string? _cachedToString;

        public string ToString(bool forceRebuild)
        {
            if (_cachedToString == null || forceRebuild == true)
            {
                var builder = new StringBuilder();
                builder.Append($"{Protocol} {StatusCode} {StatusText}");
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
}