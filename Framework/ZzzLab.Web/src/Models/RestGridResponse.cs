using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ZzzLab.Web.Models
{
    public class RestGridResponse<T> : RestResponse
    {
        /// <summary>
        /// Grid Item
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public virtual IEnumerable<T>? Items { set; get; }

        /// <summary>
        /// RecordsTotal
        /// </summary>
        [JsonProperty(PropertyName = "recordsTotal")]
        public virtual int RecordsTotal { set; get; } = 0;

        /// <summary>
        /// RecordsFiltered
        /// </summary>
        [JsonProperty(PropertyName = "recordsFiltered")]
        public virtual int RecordsFiltered { set; get; } = 0;
    }
}