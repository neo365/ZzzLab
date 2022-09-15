using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZzzLab.Web.Configuration;

namespace ZzzLab.AspCore.Common
{
    public class JwtToken : JwtSecurityTokenHandler
    {
        public string Encode(string secretKey, IReadOnlyDictionary<string, string> payloadContents, string encryptionAlgorithm)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, encryptionAlgorithm);

            var payloadClaims = payloadContents.Select(c => new Claim(c.Key, c.Value));

            var payload = new JwtPayload(payloadClaims);
            var header = new JwtHeader(signingCredentials);
            var securityToken = new JwtSecurityToken(header, payload);

            return this.WriteToken(securityToken);
        }

        public string Decode(string secretKey, string jwtEncodedString, string encryptionAlgorithm)
        {
            var parameters = new JwtSettings().GetParameters();

            return this.DecryptToken(new JwtSecurityToken(jwtEncodedString), parameters);
        }
    }
}