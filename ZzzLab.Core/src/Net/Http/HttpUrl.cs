using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace ZzzLab.Net.Http
{
    public class HttpUrl
    {
        private string _Raw = string.Empty;

        [JsonProperty(PropertyName = "raw")]
        public string Raw
        {
            get => _Raw;
            set
            {
                if (_Raw == value) return;
                _Raw = value;

                this.Parser(value);
            }
        }

        public string HostNameType { set; get; }

        [JsonProperty(PropertyName = "protocol")]
        public string Protocol => Scheme;

        [JsonProperty(PropertyName = "scheme")]
        public string Scheme { get; set; }

        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        [JsonProperty(PropertyName = "port")]
        public int Port { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "queryString")]
        public string QueryString { get; set; }

        public string PathWithQuery => $"{Path}{QueryString}";

        [JsonProperty(PropertyName = "query")]
        public IEnumerable<KeyValuePair<string, string>> Query => ToQuery();

        public static HttpUrl Create(string url)
            => new HttpUrl().Parser(url);

        public HttpUrl Parser(string rawValue)
        {
            try
            {
                Uri uri = new Uri(rawValue);
                _chashedQuery = null;
                _Raw = rawValue;
                Scheme = uri.Scheme;
                Host = uri.Host;
                HostNameType = uri.HostNameType.ToString();
                Port = uri.Port;
                Path = uri.AbsolutePath;
                QueryString = uri.Query;
            }
            catch (Exception ex)
            {
                Logger.Warning(ex);
            }

            return this;
        }

        private IEnumerable<KeyValuePair<string, string>> _chashedQuery = null;

        private IEnumerable<KeyValuePair<string, string>> ToQuery(Encoding encoding = null)
        {
            if (_chashedQuery != null) return _chashedQuery;

            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrWhiteSpace(this.QueryString) == false)
            {
                NameValueCollection queryCollection = HttpUtility.ParseQueryString(this.QueryString, encoding ?? Encoding.Default);
                if (queryCollection != null)
                {
                    foreach (string key in queryCollection.Keys)
                    {
                        list.Add(new KeyValuePair<string, string>(key, queryCollection[key] ?? string.Empty));
                    }
                }
            }
            _chashedQuery = list;
            return list;
        }

        public override string ToString()
            => this.Raw;
    }
}