using System.Net.Http;
using System.Text;

namespace ZzzLab.Net.Http
{
    public class JsonContent : StringContent
    {
        private JsonContent(string value) : base(value)
        {
            this.Headers.ContentType.MediaType = "application/json";
        }

        private JsonContent(string value, Encoding encoding) : base(value, encoding)
        {
            this.Headers.ContentType.MediaType = "application/json";
        }

        private JsonContent(string value, Encoding encoding, string mediaType) : base(value, encoding, mediaType)
        {
        }

        public static JsonContent Create(string value)
            => new JsonContent(value);

        public static JsonContent Create(string value, Encoding encoding)
            => new JsonContent(value, encoding);

        public static JsonContent Create(string value, string mediaType)
            => new JsonContent(value, Encoding.Default, mediaType);

        public static JsonContent Create<T>(T value, string mediaType = "application/json")
            => new JsonContent(Json.JsonConvert.SerializeObject(value), Encoding.Default, mediaType);
    }
}