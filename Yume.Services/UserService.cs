using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Yume.Data.Contexts;
using Yume.Data.Entities.Auth;
using Yume.Data.Entities.User;
using Yume.Data.Factories;
using Yume.Models.Session;
using Yume.Models.User;
using Yume.Services.Interfaces;

namespace Yume.Services
{
    public class UserService(PostgreDbContext context, IAuthService authService, IJwtService jwtService, ISessionService sessionService, UserFactory userFactory) : IUserService
    {
        private readonly PostgreDbContext _context = context;
        private readonly IAuthService _authService = authService;
        private readonly IJwtService _jwtService = jwtService;
        private readonly ISessionService _sessionService = sessionService;
        private readonly UserFactory _userFactory = userFactory;

        public async Task<UserModel?> RetrieveIdentity(ClaimsIdentity? identity)
        {
            Claim? sessionIdClaim = identity?.FindFirst("session_id");

            if (string.IsNullOrEmpty(sessionIdClaim?.Value)) return null;

            SessionViewModel? session = await _sessionService.GetSessionById(Guid.Parse(sessionIdClaim.Value));

            if (session?.OwnerId == null) return null;

            User? user = await RetrieveUser(session.OwnerId);

            return user != null ? MapUserToUserModel(user) : null;
        }

        public async Task<List<User>> RetrieveUsers(Guid guid)
        {
            return await _context.Users.Where(u => u.Id.Equals(guid) && u.IsActive).ToListAsync();
        }
        public async Task<List<User>> RetrieveUsers(string email)
        {
            return await _context.Users.Where(u => u.Email.Equals(email) && u.IsActive).ToListAsync();
        }
        public async Task<List<User>> RetrieveUsers(string username, string discriminator)
        {
            return await _context.Users.Where(u => u.Username.Equals(username) && u.Discriminator.Equals(discriminator) && u.IsActive).ToListAsync();
        }
        public async Task<User?> RetrieveUser(Guid guid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(guid) && u.IsActive);
        }
        public async Task<User?> RetrieveUser(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.IsActive);
        }
        public async Task<User?> RetrieveUser(string username, string discriminator)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Discriminator.Equals(discriminator) && u.IsActive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private UserModel MapUserToUserModel(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Nickname = user.Nickname,
                Discriminator = user.Discriminator,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate,
                IsActive = user.IsActive,
            };
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            List<User> rawUsers = await _context.Users.ToListAsync();
            return rawUsers.Select(MapUserToUserModel).ToList();
        }

        public async Task<UserModel?> GetUserByIdAsync(Guid guid)
        {
            User? user = await RetrieveUser(guid);
            return user != null ? MapUserToUserModel(user) : null;
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            User? user = await RetrieveUser(email);
            return user != null ? MapUserToUserModel(user) : null;
        }

        public async Task<UserModel?> GetUserByUsernameAsync(string username, string discriminator)
        {
            User? user = await RetrieveUser(username, discriminator);
            return user != null ? MapUserToUserModel(user) : null;
        }

        private async Task<string> GenerateDiscriminator(string username)
        {
            IEnumerable<string> possibleDiscriminators = Enumerable.Range(1, 10000).Select(i => i.ToString("D4"));
            List<User>? users = await _context.Users.Where(u => u.Username.ToLower().Equals(username.ToLower())).ToListAsync();
            List<string> discriminators = users?.Select(u => u.Discriminator).Order().ToList() ?? [];
            return possibleDiscriminators.Except(discriminators).ToList()[new Random().Next(1, 10000)];
        }

        public async Task<UserModel?> CreateUserAsync(UserCreateModel model)
        {
            User? existingUser = await RetrieveUser(model.Email.ToLower());

            if (existingUser != null)
                return null;

            User newUser = new()
            {
                Email = model.Email.ToLower(),
                Username = model.Username.ToLower(),
                Discriminator = await GenerateDiscriminator(model.Username),
            };

            await _context.Users.AddAsync(newUser);

            // Security
            AuthUser newAuthUser = new()
            {
                UserId = newUser.Id,
                PasswordHash = _authService.HashPassword(model.Password),
            };
            await _context.AuthUsers.AddAsync(newAuthUser);

            // Customization
            UserCustomization newUserCustomization = new()
            {
                UserId = newUser.Id,
            };
            await _context.UserCustomizations.AddAsync(newUserCustomization);

            await _context.SaveChangesAsync();

            return MapUserToUserModel(newUser);
        }

        public async Task<UserModel?> UpdateUserAsync(UserUpdateModel model)
        {
            User? user = await RetrieveUser(model.Id);

            if (user == null)
                return null;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return MapUserToUserModel(user);
        }

        public async Task<bool> DeleteUserAsync(Guid guid)
        {
            User? user = await RetrieveUser(guid);

            if (user == null)
                return false;

            // Clear sessions
            await _sessionService.DeleteSessionsByOwnerId(user.Id);

            _context.Users.Remove(user);
            _context.SaveChanges();

            return true;
        }
    }
}
