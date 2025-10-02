using Yume.Data.Entities.User;

namespace Yume.Services.Interfaces
{
    public interface IFactoryService
    {
        Task<List<User>> FactoryUsers(int count);
    }
}
