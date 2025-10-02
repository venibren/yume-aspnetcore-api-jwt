using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Yume.Enums;
using Yume.Models.Session;
using Yume.Models.User;
using Yume.Services.Interfaces;

namespace Yume.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Collections
        private readonly IMongoCollection<SessionModel> _sessionCollection;

        public SessionService(IOptions<SessionDbSettingsModel> dbSettings, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _sessionCollection = new MongoClient(dbSettings.Value.ConnectionString)
            .GetDatabase(dbSettings.Value.Name)
            .GetCollection<SessionModel>(dbSettings.Value.Collection);

            IndexKeysDefinition<SessionModel> indexKeysDefinition = Builders<SessionModel>.IndexKeys.Ascending("expireAt");
            CreateIndexModel<SessionModel> indexModel = new(indexKeysDefinition, new CreateIndexOptions { ExpireAfter = TimeSpan.Zero });
            _sessionCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<SessionViewModel?> GetSessionById(Guid id)
        {
            SessionModel? response = await _sessionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (response == null)
                return null;

            return SessionViewModel.Parse(response);
        }

        public async Task<List<SessionViewModel>> GetSessionsByOwnerId(Guid id)
        {
            List<SessionModel> response = await _sessionCollection.Find(x => x.OwnerId == id).ToListAsync();

            return response.Select(session => SessionViewModel.Parse(session)).ToList();
        }

        public async Task<SessionPaginatedModel> GetSessions(SessionFilterModel filter)
        {
            FilterDefinition<SessionModel> query = Builders<SessionModel>.Filter.Empty;

            // Session ID
            if (filter.Ids != null && filter.Ids.Count > 0)
                query &= Builders<SessionModel>.Filter.In(x => x.Id, filter.Ids);

            // Session Owner ID
            if (filter.OwnerIds != null && filter.OwnerIds.Count > 0)
                query &= Builders<SessionModel>.Filter.In(x => x.OwnerId, filter.OwnerIds);

            // Session Creation
            if (filter.CreatedFrom.HasValue)
                query &= Builders<SessionModel>.Filter.Gte(x => x.CreatedAt, filter.CreatedFrom.Value);
            if (filter.CreatedTo.HasValue)
                query &= Builders<SessionModel>.Filter.Lte(x => x.CreatedAt, filter.CreatedTo.Value);

            // Session Expiration
            if (filter.ExpiresFrom.HasValue)
                query &= Builders<SessionModel>.Filter.Gte(x => x.ExpireAt, filter.ExpiresFrom.Value);
            if (filter.ExpiresTo.HasValue)
                query &= Builders<SessionModel>.Filter.Lte(x => x.ExpireAt, filter.ExpiresTo.Value);

            // Session Type
            if (filter.Type.HasValue)
                query &= Builders<SessionModel>.Filter.Eq(x => x.Type, filter.Type.Value);

            long totalResults = await _sessionCollection.Find(query).CountDocumentsAsync();
            List<SessionModel> sessions = await _sessionCollection.Find(query).Skip((filter.Page - 1) * filter.PageSize).Limit(filter.PageSize).ToListAsync();

            return SessionPaginatedModel.Parse(new()
            {
                CurrentPage = filter.Page,
                Items = sessions.Select(session => SessionViewModel.Parse(session)).ToList(),
                PageSize = filter.PageSize,
                TotalResults = unchecked((int)totalResults)
            });
        }

        public async Task<SessionViewModel> CreateSession(UserTokenModel model)
        {
            string? ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            string? agent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();

            SessionModel session = new()
            {
                Id = model.Session,
                OwnerId = model.UserId,
                Token = model.Token,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = model.ExpiryDate,
                Type = SessionTypeEnum.User,
                IpAddress = ip,
                Agent = agent,
            };

            await _sessionCollection.InsertOneAsync(session);

            return SessionViewModel.Parse(session);
        }

        public async Task UpdateSession(Guid id, SessionModel session)
        {
            await _sessionCollection.ReplaceOneAsync(x => x.Id == id, session);
        }

        public async Task<bool> DeleteSession(Guid sessionId)
        {
            DeleteResult? response = await _sessionCollection.DeleteOneAsync(x => x.Id == sessionId);

            return response?.DeletedCount > 0;
        }

        public async Task<bool> DeleteSessionsByOwnerId(Guid ownerId)
        {
            DeleteResult? response = await _sessionCollection.DeleteManyAsync(x => x.OwnerId == ownerId && x.Type == SessionTypeEnum.User);

            return response?.DeletedCount > 0;
        }
    }
}
