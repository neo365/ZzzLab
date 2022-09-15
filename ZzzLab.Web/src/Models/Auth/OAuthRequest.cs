using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ZzzLab.Web.Models.Auth
{
    public class OAuthRequest : RequestBase
    {
        [Required(ErrorMessage = "grant_type is required.")]
        [JsonProperty(PropertyName = "grant_type")]
        public GrantType GrantType { set; get; } = GrantType.password;

        #region password

        [JsonProperty(PropertyName = "username")]
        public string? UserName { set; get; }

        [JsonProperty(PropertyName = "password")]
        public string? Password { set; get; }

        #endregion password

        #region authorization_code

        [JsonProperty(PropertyName = "code")]
        public string? Code { set; get; }

        #endregion authorization_code

        [JsonProperty(PropertyName = "client_id")]
        public string? ClientId { set; get; }

        [JsonProperty(PropertyName = "scope")]
        public string? Scope { set; get; }

        #region authorization_code

        [JsonProperty(PropertyName = "refresh_token")]
        public string? RefreshToken { set; get; }

        #endregion authorization_code
    }

    public enum GrantType : int
    {
        password = 1,
        authorization_code,
        refresh_token
    }
}