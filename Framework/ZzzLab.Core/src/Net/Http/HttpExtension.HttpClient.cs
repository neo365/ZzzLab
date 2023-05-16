using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZzzLab.Net.Http
{
    public static partial class HttpExtension
    {
        public static async Task<string> ReadAsStringAsync(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static string ReadAsString(this HttpResponseMessage response)
            => Task.Run(async () => await response.ReadAsStringAsync()).Result;

        public static async Task<T> ReadAsJsonAsync<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            string res = await response.Content.ReadAsStringAsync();

            return Json.JsonConvert.DeserializeObject<T>(res);
        }

        public static T ReadAsJson<T>(this HttpResponseMessage response)
            => Task.Run(async () => await response.ReadAsJsonAsync<T>()).Result;

        public static async Task<byte[]> ReadAsByteArrayAsync(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public static byte[] ReadAsByteArray(this HttpResponseMessage response)
            => Task.Run(async () => await response.ReadAsByteArrayAsync()).Result;

        public static async Task<Stream> ReadAsStreamAsync(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public static Stream ReadAsStream(this HttpResponseMessage response)
            => Task.Run(async () => await response.ReadAsStreamAsync()).Result;
    }
}