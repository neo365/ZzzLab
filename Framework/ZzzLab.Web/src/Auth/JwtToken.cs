using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ZzzLab.AspCore.Common
{
    public class JwtToken : JwtSecurityTokenHandler
    {
        private static readonly Lazy<JwtToken> _Instance = new Lazy<JwtToken>(() => new JwtToken());

        private static JwtToken Instance
        {
            get => _Instance.Value;
        }

        public static string GenerateToken(IReadOnlyDictionary<string, string> payloadContents)
            => Instance.Encode(payloadContents);

        public static string? DecodeToken(string token)
            => Instance.Decode(token);

        private string Encode(IReadOnlyDictionary<string, string> payloadContents)
        {
            if (Configurator.Setting?.JWTConfig == null) throw new InitializeException("JWT settings not found.");
            if (string.IsNullOrWhiteSpace(Configurator.Setting.JWTConfig.SigningKey)) throw new InitializeException("JWT settings not found.");
            if (string.IsNullOrWhiteSpace(Configurator.Setting.JWTConfig.Algorithm)) throw new InitializeException("JWT settings not found.");

            string signingKey = Configurator.Setting.JWTConfig.SigningKey;
            string encryptionAlgorithm = Configurator.Setting.JWTConfig.Algorithm;

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, encryptionAlgorithm);

            List<Claim> claims = new List<Claim>(payloadContents.Select(c => new Claim(c.Key, c.Value)));
            if (payloadContents.ContainsKey(JwtRegisteredClaimNames.Jti) == false) claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Configurator.Setting.JWTConfig.Issuer,
                audience: Configurator.Setting.JWTConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
            ); ;

            return this.WriteToken(token);
        }

        private string? Decode(string jwtEncodedString)
        {
            if (Configurator.Setting?.JWTConfig == null) throw new InitializeException("JWT settings not found.");

            TokenValidationParameters parameters = Configurator.Setting.JWTConfig.GetParameters();

            return this.DecryptToken(new JwtSecurityToken(jwtEncodedString), parameters);
        }
    }
}