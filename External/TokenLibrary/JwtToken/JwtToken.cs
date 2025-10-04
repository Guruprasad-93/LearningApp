using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TokenLibrary.EncryptDecrypt.AES;

namespace TokenLibrary.JwtToken
{
    internal class JwtTokens : ITokenTypeCaller
    {
        public JwtTokens(TokenCallerType tokenCallerType, TokenJwtOptions tokenJwtOptions)
        {
            TokenCallerType = tokenCallerType;
            JwtOptions = tokenJwtOptions;
        }

        public TokenJwtOptions JwtOptions { get; }

        public TokenCallerType TokenCallerType { get; }
        public TokenResponse Generate(TokenUserContext tokenUserContext)
        {
            TokenResponse tokenResponse = new()
            {
                Token = GenerateJwtToken(tokenUserContext),
                RefreshInterval = TimeSpan.FromMinutes(JwtOptions.RefreshTokenValidityInMinutes).TotalSeconds,
                RefreshToken = GenerateRefreshToken(),
                RefKey = GenerateAccessRefToken(tokenUserContext)
            };
            return tokenResponse;
        }

        private string GenerateAccessRefToken(TokenUserContext tokenUserContext)
        {
            long userId = tokenUserContext.UserId;
            EncryptDecryptAes.StrEncryptionKey = JwtOptions.EncryptionAlgorithmkey;

            return EncryptDecryptAes.EncryptStringAES(Convert.ToString(userId));
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            string refreshToken;

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            refreshToken = Convert.ToBase64String(randomNumber);

            return refreshToken;
           
        }

        private string GenerateJwtToken(TokenUserContext tokenUserContext)
        {
            if(tokenUserContext is null)
            {
                throw new ArgumentNullException(nameof(tokenUserContext));
            }

            if (string.IsNullOrEmpty(tokenUserContext.SessionId))
            {
                throw new ArgumentException("SessionId cannot be null or empty.");
            }

            JwtSecurityTokenHandler tokenHandler = new();

            byte[] key = Encoding.ASCII.GetBytes(JwtOptions.Secret);

            string roles = string.Empty;

            if (tokenUserContext != null && tokenUserContext.CurrentRole != null && tokenUserContext.CurrentRole.RoleCode != null)
            {
                roles = tokenUserContext.CurrentRole.RoleCode;
            }

            EncryptDecryptAes.StrEncryptionKey = JwtOptions.EncryptionAlgorithmkey;
            string encryptedContext = EncryptDecryptAes.EncryptStringAES(JsonSerializer.Serialize(tokenUserContext));

            ClaimsIdentity claimsIdentity = new();

            if (tokenUserContext != null && !string.IsNullOrEmpty(tokenUserContext.LoginId))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, tokenUserContext.LoginId));
            }

            claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, encryptedContext));

            string[] roleIdentity = roles.Split(',');

            if(roleIdentity.Length > 0)
            {
                foreach(string rolecode in roleIdentity)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, rolecode));
                }
            }

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claimsIdentity),
                Expires = DateTime.UtcNow.AddMinutes(JwtOptions.TokenValidityInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = JwtOptions.ValidAudience,
                Issuer = JwtOptions.ValidIssuer
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
