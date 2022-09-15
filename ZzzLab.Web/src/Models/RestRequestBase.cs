using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZzzLab.Web.Models
{
    public class RestRequestBase
    {
        /// <summary>
        /// i18n. 다국어 구현시 필수 값. 지역-언어 의 구성으로 이루어져 있다.
        /// 지역은 ISO 3166-1 alpha-2의 규칙을 따른다.
        /// 언어는 ISO 639 alpha-2 의 규칙을 따른다.
        /// </summary>
        [JsonProperty(PropertyName = "i18n")]
        public string I18n { get; set; } = "ko-Kr";

        /// <summary>
        /// 지역. ISO 3166-1 alpha-2의 규칙을 따른다.
        /// </summary>
        [JsonProperty(PropertyName = "locale")]
        public string? Locale
        {
            get
            {
                if (string.IsNullOrWhiteSpace(I18n)) return null;
                if (I18n.Contains('-') == false) return null;

                return I18n.Split('-')[0];
            }
        }

        /// <summary>
        /// 언어. ISO 639 alpha-2 의 규칙을 따른다.
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string? Language
        {
            get
            {
                if (string.IsNullOrWhiteSpace(I18n)) return null;
                if (I18n.Contains('-') == false) return null;

                return I18n.Split('-')[1];
            }
        }
    }
}