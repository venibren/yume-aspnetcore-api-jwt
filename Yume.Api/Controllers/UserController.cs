using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Security.Claims;
using Yume.Models.User;
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(400, "Request invalid.")]
    [SwaggerResponse(401, "Request unauthorized.")]
    [SwaggerResponse(403, "Forbidden request.")]
    [SwaggerResponse(429, "Rate limit exceeded.")]
    [SwaggerResponse(500, "Internal server error.")]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Retrieve session user.
        /// </summary>
        /// <returns>User Model</returns>
        /// <remarks>
        /// GET /Api/V1/User
        /// </remarks>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "Identity retrieved.")]
        [SwaggerResponse(404, "Identity not found.")]
        public async Task<ActionResult<UserModel>> GetUser()
        {
            try
            {
                UserModel? identity = await _userService.RetrieveIdentity(HttpContext.User.Identity as ClaimsIdentity);

                return Ok(identity);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieve a user.
        /// </summary>
        /// <param name="guid">User ID</param>
        /// <returns>User Model</returns>
        /// <remarks>
        /// GET /Api/V1/User/aa257c4b-be82-4c35-a307-6b0f61ee750f
        /// </remarks>
        [HttpGet("{guid:guid}")]
        public async Task<ActionResult<UserModel>> GetUser(Guid guid)
        {
            try
            {
                UserModel? result = await _userService.GetUserByIdAsync(guid);

                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                //_logger.LogError(ex, "User {@guid} not found", guid);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to return User for {@guid}", guid);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieve a user using the username and discriminator.
        /// Example: username#1234
        /// </summary>
        /// <param name="userTag">User ID</param>
        /// <returns>User Model</returns>
        /// <remarks>
        /// GET /Api/V1/User/username#1234
        /// </remarks>
        [HttpGet("{userTag}")]
        public async Task<ActionResult<UserModel>> GetUser(string userTag)
        {
            try
            {
                string username = userTag.Split("#")[0];
                string discriminator = userTag.Split("#")[1];

                UserModel? result = await _userService.GetUserByUsernameAsync(username, discriminator);

                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("List")]
        public async Task<ActionResult<List<UserModel>>> GetUsers()
        {
            try
            {
                List<UserModel>? result = await _userService.GetAllUsersAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Create")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CreateUser([FromForm] UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.CreateUserAsync(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateUser(UserUpdateModel model)
        {
            try
            {
                await _userService.UpdateUserAsync(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUser()
        {
            try
            {
                UserModel? identity = await _userService.RetrieveIdentity(HttpContext.User.Identity as ClaimsIdentity);

                if (identity == null)
                    return NotFound();

                bool success = await _userService.DeleteUserAsync(identity.Id);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{guid:guid}")]
        public async Task<IActionResult> DeleteUser(Guid guid)
        {
            try
            {
                bool success = await _userService.DeleteUserAsync(guid);

                return Ok(success);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
