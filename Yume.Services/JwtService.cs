using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Yume.Data.Entities.Client;
using Yume.Data.Entities.User;
using Yume.Enums;
using Yume.Models.Client;
using Yume.Models.User;
using Yume.Services.Interfaces;

namespace Yume.Services
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;

        public ClientTokenModel GenerateClientJwt(Client client, Guid ownerId)
        {
            int expireHours = int.Parse(_configuration["Security:Jwt:ExpireHours"] ?? "0");
            int expireMinutes = int.Parse(_configuration["Security:Jwt:ExpireMinutes"] ?? "0");
            DateTime expires = DateTime.UtcNow.AddHours(expireHours).AddMinutes(expireMinutes);

            string secret = _configuration["Security:Jwt:Secret"] ?? string.Empty;
            byte[] key = Encoding.UTF8.GetBytes(secret);

            // MongoDb Session Identifier
            Guid sessionId = Guid.NewGuid();

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("session_id", sessionId.ToString()),
                    new Claim(ClaimTypes.Name, client.Name),
                    new Claim("type", SessionTypeEnum.Client.ToString())
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return new ClientTokenModel
            {
                OwnerId = ownerId,
                Session = sessionId,
                Token = tokenString,
                ExpiryDate = expires
            };
        }

        public UserTokenModel GenerateUserJwt(User user)
        {
            int expireHours = int.Parse(_configuration["Security:Jwt:ExpireHours"] ?? "0");
            int expireMinutes = int.Parse(_configuration["Security:Jwt:ExpireMinutes"] ?? "0");
            DateTime expires = DateTime.UtcNow.AddHours(expireHours).AddMinutes(expireMinutes);

            string secret = _configuration["Security:Jwt:Secret"] ?? string.Empty;
            byte[] key = Encoding.UTF8.GetBytes(secret);

            // MongoDb Session Identifier
            Guid sessionId = Guid.NewGuid();

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("session_id", sessionId.ToString()),
                    new Claim(ClaimTypes.Name, user.ToString()),
                    new Claim("type", SessionTypeEnum.User.ToString()),
                    new Claim(ClaimTypes.Role, "admin") // Add roles and policies
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return new UserTokenModel
            {
                UserId = user.Id,
                Session = sessionId,
                Token = tokenString,
                ExpiryDate = expires
            };
        }
    }
}
