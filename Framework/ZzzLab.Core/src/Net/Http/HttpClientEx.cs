using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ZzzLab.Net.Http
{
    public class HttpClientEx
    {
        private HttpClient Client { get; } = new HttpClient();

        public HttpClientEx(string baseUrl = null, string userAgent = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) == false)
            {
                Client.BaseAddress = new Uri(baseUrl);
            }
            if (string.IsNullOrWhiteSpace(userAgent)) userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36";

            Client.DefaultRequestHeaders.Add("user-agent", userAgent);
        }

        public static HttpClientEx Create(string baseUrl = null, string userAgent = null)
            => new HttpClientEx(baseUrl, userAgent);

        /// <summary>
        /// Http Get 으로 문자열을 가져온다.
        /// </summary>
        /// <param name="url">접속url</param>
        /// <param name="parameters">파라미터</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">url이 없을 경우</exception>
        public async Task<HttpResponseMessage> GetAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            if (parameters != null && parameters.Any())
            {
                string queryString = (url.LastIndexOf('?') > 0 ? "&" : "?") + ToQueryString(parameters);
                url = $"{url}{queryString}";
            }

            HttpResponseMessage res = await this.Client.GetAsync(url);

            return res;
        }

        public HttpResponseMessage Get(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
            => Task.Run(async () => await GetAsync(url, parameters)).Result;

        public async Task<HttpResponseMessage> PostAsync(string url, string value, string mediaType = "application/text")
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(mediaType)) throw new ArgumentNullException(nameof(mediaType));

            StringContent content = new StringContent(value);
            content.Headers.ContentType.MediaType = mediaType;

            HttpResponseMessage response = await this.Client.PostAsync(url, content);
            return response;
        }

        public HttpResponseMessage Post(string url, IEnumerable<KeyValuePair<string, string>> parameters)
            => Task.Run(async () => await PostAsync(url, ToQueryString(parameters), "application/x-www-form-urlencoded")).Result;

        public HttpResponseMessage PostRaw<T>(string url, string obj, string mediaType = null)
        {
            if (string.IsNullOrWhiteSpace(mediaType)) mediaType = "application/text";
            string parameter = null;
            if (obj is string value) parameter = value;
            else parameter = obj.ToString();

            return Task.Run(async () => await PostAsync(url, parameter, mediaType)).Result;
        }

        public HttpResponseMessage PostJson<T>(string url, T obj, JsonSerializerSettings settings = null, string mediaType = null)
        {
            if (string.IsNullOrWhiteSpace(mediaType)) mediaType = "application/json";

            string parameter = null;
            if (obj is string value) parameter = value;
            else parameter = Json.JsonConvert.SerializeObject(obj, settings);

            return Task.Run(async () => await PostAsync(url, parameter, mediaType)).Result;
        }

        private string ToQueryString(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                string queryString = "";
                foreach (var item in parameters)
                {
                    queryString += $"&{HttpUtility.UrlEncode(item.Key)}={HttpUtility.UrlEncode(item.Value)}";
                }
                return queryString.TrimStart('&');
            }

            return null;
        }
    }
}