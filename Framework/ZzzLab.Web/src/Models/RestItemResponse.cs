using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ZzzLab.Web.Models
{
    public class RestItemResponse<T> : RestResponse
    {
        /// <summary>
        /// Item
        /// </summary>
        [JsonProperty(PropertyName = "item")]
        [XmlElement(ElementName = "item")]
        public virtual T? Item { set; get; }
    }
}