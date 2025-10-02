using Yume.Data.Entities.Client;
using Yume.Data.Entities.User;
using Yume.Models.Client;
using Yume.Models.User;

namespace Yume.Services.Interfaces
{
    public interface IJwtService
    {
        ClientTokenModel GenerateClientJwt(Client client, Guid ownerId);
        UserTokenModel GenerateUserJwt(User user);
    }
}
