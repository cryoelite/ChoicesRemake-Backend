using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StaticAssets;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.Services
{
    public class JWTDecryptor
    {
        public JWTSettings jwtOptions;

        public JWTDecryptor(IOptions<JWTSettings> options) => (jwtOptions) = (options.Value);

        public string? GetUsername(string token)
        {
            try
            {
                var signingKey = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
                var encKey = Encoding.UTF8.GetBytes(jwtOptions.EncryptionKey);
                var byteArray = new byte[32];
                Array.Copy(encKey, byteArray, 32);

                var encSigningKey = new SymmetricSecurityKey(signingKey);
                var encEncKey = new SymmetricSecurityKey(byteArray);
                var handler = new JwtSecurityTokenHandler();
                var claim = handler.ValidateToken(token, new TokenValidationParameters()
                { TokenDecryptionKey = encEncKey, IssuerSigningKey = encSigningKey, ValidAudience = jwtOptions.Issuer, ValidIssuer = jwtOptions.Issuer }, out SecurityToken securityToken);
                var username = claim.FindFirst(ClaimTypes.Email);
                return username?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}