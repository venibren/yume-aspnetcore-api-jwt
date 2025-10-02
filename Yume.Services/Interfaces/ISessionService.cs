using Yume.Models.Session;
using Yume.Models.User;

namespace Yume.Services.Interfaces
{
    /// <summary>
    /// Maintain sessions for jwt authorization
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Retrieve session details by Id
        /// </summary>
        /// <param name="id">Guid session identifier</param>
        /// <returns>Session details</returns>
        Task<SessionViewModel?> GetSessionById(Guid id);

        /// <summary>
        /// Retrieve sessions by Owner Id
        /// </summary>
        /// <param name="id">Guid owner identifier</param>
        /// <returns>List of session details</returns>
        Task<List<SessionViewModel>> GetSessionsByOwnerId(Guid id);

        /// <summary>
        /// Retrieve sessions by filter model
        /// </summary>
        /// <param name="filter">Filter parameters</param>
        /// <returns>Paginated session results</returns>
        Task<SessionPaginatedModel> GetSessions(SessionFilterModel filter);

        /// <summary>
        /// Create a new session
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Token details</returns>
        Task<SessionViewModel> CreateSession(UserTokenModel model);

        /// <summary>
        /// Update a single session by session Id
        /// </summary>
        /// <param name="id">Guid session identifier</param>
        /// <param name="session">Session model</param>
        Task UpdateSession(Guid id, SessionModel session);

        /// <summary>
        /// Delete session by Id
        /// </summary>
        /// <param name="sessionId">Active session guid</param>
        /// <returns>Success Bool</returns>
        Task<bool> DeleteSession(Guid sessionId);

        /// <summary>
        /// Delete all sessions by owner guid
        /// </summary>
        /// <param name="ownerId">Guid owner identifier</param>
        /// <returns>Success bool</returns>
        Task<bool> DeleteSessionsByOwnerId(Guid userId);
    }
}
