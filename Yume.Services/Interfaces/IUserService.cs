using System.Security.Claims;
using Yume.Data.Entities.User;
using Yume.Models.User;

namespace Yume.Services.Interfaces
{
    public interface IUserService
    {
        // Identities
        Task<UserModel?> RetrieveIdentity(ClaimsIdentity? identity);

        // User retrieval
        Task<List<User>> RetrieveUsers(Guid guid);
        Task<List<User>> RetrieveUsers(string email);
        Task<List<User>> RetrieveUsers(string username, string discriminator);
        Task<User?> RetrieveUser(Guid guid);
        Task<User?> RetrieveUser(string email);
        Task<User?> RetrieveUser(string username, string discriminator);

        // User search
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel?> GetUserByIdAsync(Guid guid);
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task<UserModel?> GetUserByUsernameAsync(string username, string discriminator);

        // User actions
        Task<UserModel?> CreateUserAsync(UserCreateModel model);
        Task<UserModel?> UpdateUserAsync(UserUpdateModel model);
        Task<bool> DeleteUserAsync(Guid guid);
    }
}
