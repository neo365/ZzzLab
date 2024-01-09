using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ZzzLab.Web.Configuration
{
    public class JWTConfig
    {
        protected bool ValidateSigningKey => (string.IsNullOrWhiteSpace(SigningKey) == false);

        [JsonProperty("signingKey")]
        [JsonPropertyName("signingKey")]
        public string SigningKey { get; set; } = "1234567890123456789012345678901234567890123456789012345678901234";
        protected bool ValidateIssuer => (string.IsNullOrWhiteSpace(Issuer) == false);

        [JsonProperty(nameof(Issuer))]
        [JsonPropertyName(nameof(Issuer))]
        public string Issuer { get; set; } = "http://auth.zzzlab.net";
        protected bool ValidateAudience => (string.IsNullOrWhiteSpace(Audience) == false);

        [JsonProperty(nameof(Audience))]
        [JsonPropertyName(nameof(Audience))]
        public string Audience { get; set; } = "http://auth.zzzlab.net";

        [JsonProperty(nameof(Algorithm))]
        [JsonPropertyName(nameof(Algorithm))]
        public string Algorithm { set; get; } = SecurityAlgorithms.HmacSha512;

        public TokenValidationParameters GetParameters()
        {
            string signingKey = this.SigningKey?.PadRight(64) ?? $"{Guid.NewGuid()}{Guid.NewGuid()}".Remove("-");

            switch (this.Algorithm)
            {
                case SecurityAlgorithms.HmacSha256:
                case SecurityAlgorithms.HmacSha512:
                    signingKey = this.SigningKey?.PadRight(64) ?? $"{Guid.NewGuid()}{Guid.NewGuid()}".Remove("-"); ;
                    break; ;
                default:
                    break;
            }

            TokenValidationParameters parameters = new TokenValidationParameters();

            if (parameters.ValidateIssuerSigningKey = this.ValidateSigningKey) parameters.IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey));
            if (parameters.ValidateIssuer = this.ValidateIssuer) parameters.ValidIssuer = this.Issuer;
            if (parameters.ValidateAudience = this.ValidateAudience) parameters.ValidAudience = this.Audience;

            parameters.RequireExpirationTime = true;
            parameters.ValidateLifetime = true;
            parameters.ClockSkew = TimeSpan.Zero;

            return parameters;
        }
    }
}