using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZzzLab.Net.Http
{
    public class HttpClientHelper
    {
        /// <summary>
        /// Http Get 으로 문자열을 가져온다.
        /// </summary>
        /// <param name="url">접속url</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">url이 없을 경우</exception>
        public static async Task<string> GetStringAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        /// <summary>
        /// Http Get 으로 문자열을 가져온다.
        /// </summary>
        /// <param name="url">접속url</param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        public static string GetString(string url, bool throwOnError = true)
        {
            if (throwOnError) return Task.Run(async () => await GetStringAsync(url)).Result;

            try
            {
                return Task.Run(async () => await GetStringAsync(url)).Result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// Http Get 으로 binary를 가져온다.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">url이 없을 경우</exception>
        public static async Task<byte[]> GetBytesAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
        }

        public static byte[] GetBytes(string url, bool throwOnError = true)
        {
            if (throwOnError) return Task.Run(async () => await GetBytesAsync(url)).Result;

            try
            {
                return Task.Run(async () => await GetBytesAsync(url)).Result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }
    }
}