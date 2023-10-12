using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace ZzzLab.Json
{
    /// <summary>
    ///
    /// </summary>
    public static class JsonConvert
    {
        private static readonly JsonSerializerSettings _BaseSetting = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// json파일을 지정된 형식으로 변환한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeObjectFromFile<T>(string filepath, string path = "")
            => DeserializeObject<T>(File.ReadAllText(filepath), path);

        /// <summary>
        /// json파일을 지정된 형식으로 변환한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static T DeserializeObject<T>(string json, string path = null)
        {
            if (string.IsNullOrWhiteSpace(path)) return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            else
            {
                JToken jtoken = JObject.Parse(json)?.SelectToken(path);
                return jtoken == null ? throw new NullReferenceException() : jtoken.ToObject<T>();
            }
        }

        /// <summary>
        /// json파일을 지정된 형식으로 변환한다.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static dynamic DeserializeObject(string json, string parent = "")
        {
            if (string.IsNullOrWhiteSpace(parent)) return Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            else
            {
                JToken jtoken = JObject.Parse(json)?.SelectToken(parent);

                if (jtoken == null) return null;

                return Newtonsoft.Json.JsonConvert.DeserializeObject(jtoken.ToString());
            }
        }

        /// <summary>
        /// json에서 값을 가져온다.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetValue(string json, string path)
        {
            JToken jtoken = JObject.Parse(json)?.SelectToken(path);
            if (jtoken == null) return null; ;
            return jtoken.Value<string>();
        }

        /// <summary>
        /// object를 json으로 변환한다.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns>json 문자열</returns>
        public static string SerializeObject(this object obj, JsonSerializerSettings settings = null)
        {
            settings = settings ?? _BaseSetting;

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);

            return (result != null && result.EqualsIgnoreCase("null") ? null : result);
        }

        /// <summary>
        /// object를 json으로 변환한다.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns>json 문자열</returns>
        public static string ToJson<T>(this T obj, JsonSerializerSettings settings = null) where T : class
            => SerializeObject(obj, settings);

        /// <summary>
        /// object를 json으로 저장한다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        public static void Save(string filePath, object obj, JsonSerializerSettings settings = null)
            => File.WriteAllText(filePath, SerializeObject(obj, settings));

        /// <summary>
        /// json을 이쁘게 줄맞춤 해준다.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string Beautify(this string json, JsonSerializerSettings settings = null)
            => Newtonsoft.Json.JsonConvert.SerializeObject(Newtonsoft.Json.JsonConvert.DeserializeObject(json), settings);
    }
}