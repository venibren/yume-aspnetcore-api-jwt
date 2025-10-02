using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Yume.Exceptions.Auth;
using Yume.Exceptions.User;
using Yume.Models.Auth;
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(400, "Request invalid.")]
    [SwaggerResponse(403, "Forbidden request.")]
    [SwaggerResponse(429, "Rate limit exceeded.")]
    [SwaggerResponse(500, "Internal server error.")]
    public class AuthController(IAuthService authService, IUserService userService) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly IUserService _userService = userService;

        [AllowAnonymous]
        [HttpPost("Signup")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(200, "Signup successful.")]
        [SwaggerResponse(409, "The user already exists.")]
        public async Task<ActionResult<UserModel?>> Signup([FromForm] UserCreateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (await _userService.RetrieveUser(model.Email) != null)
                    return Conflict($"The user already exists with the email: '{model.Email}'.");

                UserModel? response = await _userService.CreateUserAsync(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // TODO: Create a Prepare Signin to verify a user is found and retrieve mfa settings

        [AllowAnonymous]
        [HttpPost("Signin")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "Signin successful.")]
        [SwaggerResponse(404, "The user was not found.")]
        public async Task<ActionResult<UserTokenModel>> Signin([FromForm] AuthSigninRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                UserTokenModel? response = await _authService.SigninAsync(model.Login, model.Password, model.Otp);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AuthPasswordException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (AuthMfaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Refresh")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerResponse(200, "Refresh successful.")]
        [SwaggerResponse(204, "No active session found for user.")]
        public async Task<ActionResult<UserTokenModel>> RefreshToken()
        {
            try
            {
                Guid? sessionId = _authService.RetrieveSessionId(HttpContext.User.Identity as ClaimsIdentity);

                UserTokenModel? response = null;
                if (sessionId != null && sessionId != Guid.Empty) response = await _authService.RefreshAsync((Guid)sessionId);
                if (response == null) return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Signout"), HttpDelete("Signout")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerResponse(200, "User signed out successfully.")]
        [SwaggerResponse(204, "No active session found for user.")]
        public async Task<ActionResult<bool>> Signout()
        {
            try
            {
                Guid? sessionId = _authService.RetrieveSessionId(HttpContext.User.Identity as ClaimsIdentity);

                if (sessionId == null)
                    return NoContent();

                bool success = await _authService.SignOutAsync(sessionId ?? Guid.Empty);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPost("Totp")]
        //[MapToApiVersion("1.0")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[SwaggerResponse(200, "Totp successful.")]
        //[SwaggerResponse(404, "The user not found.")]
        //public async Task<ActionResult<UserTokenModel>> TotpUser([FromForm] AuthTotpModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    try
        //    {
        //        //UserTokenModel? response = await _authService.TotpAsync();

        //        return Ok();
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpPost("Backup")]
        //[MapToApiVersion("1.0")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[SwaggerResponse(200, "Backup successful.")]
        //[SwaggerResponse(404, "The user not found.")]
        //public async Task<ActionResult<UserTokenModel>> BackupUser([FromForm] AuthBackupRequestModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    try
        //    {
        //        //UserTokenModel? response = await _authService.BackupAsync();

        //        return Ok();
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
