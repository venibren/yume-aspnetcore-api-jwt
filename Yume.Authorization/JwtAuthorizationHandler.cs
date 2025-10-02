using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Claims;
using Yume.Models.Session;

namespace Yume.Authorization
{
    public class JwtAuthorizationHandler(IOptions<SessionDbSettingsModel> dbSettings) : AuthorizationHandler<IAuthorizationRequirement>
    {
        // Collections
        private readonly IMongoCollection<SessionModel> _sessionCollection = new MongoClient(dbSettings.Value.ConnectionString)
            .GetDatabase(dbSettings.Value.Name)
            .GetCollection<SessionModel>(dbSettings.Value.Collection);


        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            // If Authentication / Jwt is invalid
            if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // If Session Id is not found within Jwt Claims
            if (!Guid.TryParse((context.User.Identity as ClaimsIdentity)?.FindFirst("session_id")?.Value, out Guid sessionId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Find active session using Session Id
            SessionModel? session = await _sessionCollection.Find(x => x.Id == sessionId).FirstOrDefaultAsync();

            // If no session found
            // If session created timestamp is later than now
            // if session is already expired
            if (session == null || session.CreatedAt >= DateTime.UtcNow || session.ExpireAt <= DateTime.UtcNow)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
