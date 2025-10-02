using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Security.Principal;
using Yume.Exceptions;
using Yume.Models.Session;
using Yume.Services.Interfaces;

namespace Yume.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "V1")]
    [Route("Api/V1/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(400, "Request invalid.")]
    [SwaggerResponse(403, "Forbidden request.")]
    [SwaggerResponse(429, "Rate limit exceeded.")]
    [SwaggerResponse(500, "Internal server error.")]
    public class SessionController(ISessionService sessionService) : Controller
    {
        private readonly ISessionService _sessionService = sessionService;

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(200, "")]
        [SwaggerResponse(404, "")]
        public async Task<ActionResult<SessionViewModel?>> Session()
        {
            try
            {
                ClaimsIdentity user = HttpContext.User.Identity as ClaimsIdentity;
                Claim? sessionIdClaim = user?.FindFirst("session_id");

                if (string.IsNullOrEmpty(sessionIdClaim?.Value))
                    return null;

                SessionViewModel? sessionDetails = await _sessionService.GetSessionById(Guid.Parse(sessionIdClaim.Value));

                return Ok(sessionDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("List")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(200, "")]
        [SwaggerResponse(404, "")]
        public async Task<ActionResult<List<SessionViewModel>>> Sessions()
        {
            try
            {
                ClaimsIdentity user = HttpContext.User.Identity as ClaimsIdentity;
                Claim? sessionIdClaim = user?.FindFirst("session_id");

                if (string.IsNullOrEmpty(sessionIdClaim?.Value))
                    return null;

                SessionViewModel? sessionDetails = await _sessionService.GetSessionById(Guid.Parse(sessionIdClaim.Value));

                var ownerId = sessionDetails?.OwnerId;

                if (ownerId == null || ownerId == Guid.Empty)
                    return null;

                List<SessionViewModel> sessions = await _sessionService.GetSessionsByOwnerId((Guid)ownerId);

                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{guid:guid}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(200, "")]
        [SwaggerResponse(404, "")]
        public async Task<ActionResult<bool>> DeleteSession(Guid guid)
        {
            try
            {
                bool success = await _sessionService.DeleteSession(guid);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}