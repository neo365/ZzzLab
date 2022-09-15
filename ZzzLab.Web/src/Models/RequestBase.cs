using Newtonsoft.Json;

namespace ZzzLab.Web.Models
{
    /// <summary>
    /// Web Request대한 기본 모델링 구현
    /// </summary>
    public class RequestBase
    {
        /// <summary>
        /// i18n. 다국어 구현시 필수 값. 지역-언어 의 구성으로 이루어져 있다.
        /// 지역은 ISO 3166-1 alpha-2의 규칙을 따른다.
        /// 언어는 ISO 639 alpha-2 의 규칙을 따른다.
        /// </summary>
        [JsonProperty(PropertyName = "I18n")]
        public string? I18n { get; set; } = "ko-KR";

        private string? _CashedLocale = null;

        /// <summary>
        /// 지역. ISO 3166-1 alpha-2의 규칙을 따른다.
        /// </summary>
        public string? Locale
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CashedLocale))
                {
                    if (string.IsNullOrWhiteSpace(I18n)) return null;
                    if (I18n.ContainsOrIgnoreCase("-") == false) return null;

                    _CashedLocale = I18n.Split('-')[1].ToUpper();
                }

                return _CashedLocale;
            }
        }

        private string? _CashedLanguage = null;

        /// <summary>
        /// 언어. ISO 639 alpha-2 의 규칙을 따른다.
        /// </summary>
        public string? Language
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CashedLanguage))
                {
                    if (string.IsNullOrWhiteSpace(I18n)) return null;
                    if (I18n.ContainsOrIgnoreCase("-") == false) return null;

                    _CashedLanguage = I18n.Split('-')[0].ToLower();
                }

                return _CashedLanguage;
            }
        }
    }
}