using Microsoft.IdentityModel.Tokens;

namespace ZzzLab.Web.Configuration
{
    public class JwtSettings
    {
        public bool ValidateIssuerSigningKey => (string.IsNullOrWhiteSpace(IssuerSigningKey) == false);
        public string IssuerSigningKey { get; set; } = string.Empty;
        public bool ValidateIssuer => (string.IsNullOrWhiteSpace(Issuer) == false);
        public string Issuer { get; set; } = string.Empty;
        public bool ValidateAudience => (string.IsNullOrWhiteSpace(Audience) == false);
        public string Audience { get; set; } = string.Empty;
        public bool RequireExpirationTime { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;

        public string? EncryptionAlgorithm { set; get; }

        public TokenValidationParameters GetParameters()
        {
            TokenValidationParameters parameters = new TokenValidationParameters();

            if (parameters.ValidateIssuerSigningKey = this.ValidateIssuerSigningKey) parameters.IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.IssuerSigningKey));
            if (parameters.ValidateIssuer = this.ValidateIssuer) parameters.ValidIssuer = this.Issuer;
            if (parameters.ValidateAudience = this.ValidateAudience) parameters.ValidAudience = this.Audience;

            parameters.RequireExpirationTime = true;
            parameters.ValidateLifetime = true;
            parameters.ClockSkew = TimeSpan.FromDays(1);

            return parameters;
        }
    }
}