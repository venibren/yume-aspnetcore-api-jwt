using Yume.Data.Contexts;
using Yume.Data.Entities.User;
using Yume.Data.Factories;
using Yume.Services.Interfaces;

namespace Yume.Services
{
    public class FactoryService(PostgreDbContext context, UserFactory userFactory): IFactoryService
    {
        private readonly PostgreDbContext _context = context;
        private readonly UserFactory _userFactory = userFactory;

        public async Task<List<User>> FactoryUsers(int count)
        {
            List<User> users = _userFactory.GenerateUsers(count);

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            return users;
        }
    }
}
