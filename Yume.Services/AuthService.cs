using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtpNet;
using System.Security.Claims;
using Yume.Attributes;
using Yume.Data.Contexts;
using Yume.Data.Entities.Auth;
using Yume.Data.Entities.User;
using Yume.Enums;
using Yume.Exceptions.Auth;
using Yume.Exceptions.User;
using Yume.Models.Session;
using Yume.Models.User;
using Yume.Services.Interfaces;

namespace Yume.Services
{
    public class AuthService(IConfiguration configuration, PostgreDbContext context, IHttpContextAccessor httpContextAccessor, IJwtService jwtService, ISessionService sessionService) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly PostgreDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtService _jwtService = jwtService;
        private readonly ISessionService _sessionService = sessionService;

        private readonly string _pepper = configuration["Security:User:Pepper"] ?? string.Empty;

        public Guid? RetrieveSessionId(ClaimsIdentity? identity)
        {
            string? value = identity?.FindFirst("session_id")?.Value;

            if (!string.IsNullOrEmpty(value) && Guid.TryParse(value, out Guid sessionId))
            {
                return sessionId;
            }

            return null;
        }

        public string HashPassword(string password)
        {
            int saltRounds = int.Parse(_configuration["Security:User:SaltRounds"] ?? "12");
            string salt = BCrypt.Net.BCrypt.GenerateSalt(saltRounds);

            // TODO: Look into the max length of BCrypt with the addition of pepper
            return BCrypt.Net.BCrypt.HashPassword(password + _pepper, salt);
        }

        protected bool ValidatePassword(AuthUser? authUser, string password)
        {
            if (authUser == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password + _pepper, authUser.PasswordHash);
        }

        protected static bool ValidateTotp(string code, string secret)
        {
            if (string.IsNullOrEmpty(secret)) return false;

            var totp = new Totp(Base32Encoding.ToBytes(secret));

            return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }

        public async Task<UserTokenModel> SigninAsync(string login, string password, string? otp)
        {
            User? user;

            // Check login if it's using the full username
            // Todo - Refer to Regex in Commons
            if (new FullUsernameAttribute().IsValid(login))
            {
                string username = login.Split("#")[0];
                string discriminator = login.Split("#")[1];
                user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Discriminator.Equals(discriminator) && u.IsActive);
            }
            // Else fallback to email
            else
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(login) && u.IsActive);
            }

            if (user == null)
            {
                throw new UserNotFoundException();
            }
            else if (!ValidatePassword(user.Auth, password))
            {
                await AddLoginFailRecordAsync(user.Auth.Id, "Invalid password attempt.");
                throw new AuthPasswordException("Invalid password.");
            }
            else if (user.Auth.MfaTotpEnabled && string.IsNullOrEmpty(otp))
            {
                await AddLoginFailRecordAsync(user.Auth.Id, "Missing one-time passcode on signin request.");
                throw new AuthMfaException("Missing one-time passcode on signin request.");
            }
            else if (user.Auth.MfaTotpEnabled && user.Auth.MfaTotp != null && !string.IsNullOrEmpty(otp) && !ValidateTotp(otp, user.Auth.MfaTotp.SecretHash))
            {
                await AddLoginFailRecordAsync(user.Auth.Id, "Invalid one-time passcode.");
                throw new AuthMfaException("Invalid one-time passcode.");
            }

            // Generate user jwt
            UserTokenModel userToken = _jwtService.GenerateUserJwt(user);

            // Store session securely
            await _sessionService.CreateSession(userToken);

            // Login history record
            await AddLoginSuccessRecordAsync(user.Auth.Id);

            return userToken;
        }

        public async Task<UserTokenModel?> RefreshAsync(Guid sessionId)
        {
            // Retrieve session
            SessionViewModel? session = await _sessionService.GetSessionById(sessionId);
            if (session == null) return null;

            // Retrive owner details
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(session.OwnerId) && u.IsActive);
            if (user == null) return null;

            // Generate new jwt
            UserTokenModel userToken = _jwtService.GenerateUserJwt(user);

            // Update session details in MongoDb, renewing token and expiration
            await _sessionService.UpdateSession(session.Id, new()
            {
                Id = sessionId,
                OwnerId = user.Id,
                Token = userToken.Token,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = userToken.ExpiryDate,
                Type = SessionTypeEnum.User,
                IpAddress = session.IpAddress,
                Agent = session.Agent,
            });

            // Return new token
            return userToken;
        }

        public async Task<bool> SignOutAsync(Guid sessionId)
        {
            return await _sessionService.DeleteSession(sessionId);
        }

        public async Task AddLoginSuccessRecordAsync(Guid authId) =>
            await AddLoginRecordAsync(authId, true);

        public async Task AddLoginFailRecordAsync(Guid authId, string reason) =>
            await AddLoginRecordAsync(authId, false, reason);

        private async Task AddLoginRecordAsync(Guid authId, bool success, string? reason = null)
        {
            string? ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            string? agent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();

            AuthUserHistory record = new()
            {
                UserId = authId,
                Success = success,
                Reason = reason,
                IpAddress = ip ?? string.Empty,
                Agent = agent ?? string.Empty
            };

            await _context.AuthUserHistory.AddAsync(record);

            // Log retention
            List<AuthUserHistory> oldLogs = await _context.AuthUserHistory.Where(x => x.Timestamp < DateTimeOffset.UtcNow.AddMonths(-3)).ToListAsync();
            if (oldLogs.Count > 0)
                _context.AuthUserHistory.RemoveRange(oldLogs);

            await _context.SaveChangesAsync();
        }
    }
}
