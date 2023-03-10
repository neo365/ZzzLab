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
        private HttpClient Client { set; get; }

        public HttpClientEx(string baseUrl = null, string userAgent = null)
        {
            HttpClient client = new HttpClient();

            if (string.IsNullOrWhiteSpace(baseUrl) == false)
            {
                client.BaseAddress = new Uri(baseUrl);
            }
            if (string.IsNullOrWhiteSpace(userAgent)) userAgent = "ZzzLab Agent 0.1";

            client.DefaultRequestHeaders.Add("user-agent", userAgent);
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

        public async Task<HttpResponseMessage> PostAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            HttpContent content = null;

            if (parameters != null && parameters.Any())
            {
                string queryString = "";
                foreach (var item in parameters)
                {
                    queryString = $"&{HttpUtility.UrlEncode(item.Key)}={HttpUtility.UrlEncode(item.Value)}";
                }
                queryString = queryString.TrimStart('&');

                content = new StringContent(queryString);
                content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            }

            HttpResponseMessage res = await this.Client.PostAsync(url, content);
            res.EnsureSuccessStatusCode();

            return res;
        }

        public HttpResponseMessage Post(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
            => Task.Run(async () => await PostAsync(url, parameters)).Result;

        public async Task<HttpResponseMessage> PostAsync(string url, string value, string mediaType = "application/text")
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            StringContent content = new StringContent(value);
            content.Headers.ContentType.MediaType = mediaType;

            HttpResponseMessage response = await this.Client.PostAsync(url, content);
            return response;
        }

        public HttpResponseMessage Post(string url, string value, string mediaType = "application/text")
            => Task.Run(async () => await PostAsync(url, value, mediaType)).Result;

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T value)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            HttpResponseMessage response = await this.Client.PostAsync(url, JsonContent.Create(value));
            return response;
        }

        public HttpResponseMessage Post<T>(string url, T value)
            => Task.Run(async () => await PostAsync(url, value)).Result;

        private string ToQueryString(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                string queryString = "";
                foreach (var item in parameters)
                {
                    queryString = $"&{HttpUtility.UrlEncode(item.Key)}={HttpUtility.UrlEncode(item.Value)}";
                }
                return queryString.TrimStart('&');
            }

            return null;
        }
    }
}