using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ZzzLab.AspCore.Common
{
    public class JwtToken : JwtSecurityTokenHandler
    {
        public string? Encode(IReadOnlyDictionary<string, string> payloadContents)
        {
            if (Configurator.Setting?.JWTConfig == null) return null;
            if (string.IsNullOrWhiteSpace(Configurator.Setting.JWTConfig.SigningKey)) return null;
            if (string.IsNullOrWhiteSpace(Configurator.Setting.JWTConfig.EncryptionAlgorithm)) return null;

            string signingKey = Configurator.Setting.JWTConfig.SigningKey;
            string encryptionAlgorithm = Configurator.Setting.JWTConfig.Algorithm;

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, encryptionAlgorithm);

            IEnumerable<Claim> claims = payloadContents.Select(c => new Claim(c.Key, c.Value));

            JwtPayload payload = new JwtPayload(claims);
            JwtHeader header = new JwtHeader(signingCredentials);
            JwtSecurityToken securityToken = new JwtSecurityToken(header, payload);

            return this.WriteToken(securityToken);
        }

        public string? Decode(string jwtEncodedString)
        {
            if (Configurator.Setting?.JWTConfig == null) return null;

            TokenValidationParameters parameters = Configurator.Setting.JWTConfig.GetParameters();

            return this.DecryptToken(new JwtSecurityToken(jwtEncodedString), parameters);
        }
    }
}