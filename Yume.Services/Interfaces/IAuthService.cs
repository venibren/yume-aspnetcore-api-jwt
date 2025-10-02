using System.Security.Claims;
using Yume.Models.User;

namespace Yume.Services.Interfaces
{
    public interface IAuthService
    {
        Guid? RetrieveSessionId(ClaimsIdentity? identity);

        string HashPassword(string input);

        Task<UserTokenModel> SigninAsync(string login, string password, string? otp);
        Task<UserTokenModel?> RefreshAsync(Guid sessionId);
        Task<bool> SignOutAsync(Guid sessionId);
        Task AddLoginSuccessRecordAsync(Guid userId);
        Task AddLoginFailRecordAsync(Guid userId, string reason);
    }
}
