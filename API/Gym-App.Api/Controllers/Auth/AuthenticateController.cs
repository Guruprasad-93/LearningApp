using gym_app.Business;
using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Model;
using gym_app.Domain.Model.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using TokenLibrary.EncryptDecrypt.AES;
using TokenLibrary.JwtToken;

namespace Gym_App.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthenticateController : BaseApiController<AuthenticateController>
    {
        private readonly JwtOptions JwtOptions;
        private readonly IAuthService authService;
        private readonly ApplicationDbContext context;
        public AuthenticateController(IAuthService _authService, ILogger<AuthenticateController> _logger, AppOptions appOptions, ApplicationDbContext _context) : base(appOptions, _logger)
        {
            authService = _authService;
            JwtOptions = appOptions.JwtOptions;
            context = _context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("loginuser")]
        public async Task<ActionResult<AuthenticateResponseModel>> LoginAsync([FromBody]AuthenticateRequestModel authenticateRequestModel)
        {
            AuthenticateResponseModel authenticateResponseModel = null;

            string LoginName = authenticateRequestModel.LoginName.Trim().ToLowerInvariant();

            using(logger.BeginScope("Authentication: {header}", Request.Headers))
            {
                if (string.IsNullOrEmpty(LoginName))
                {
                    logger.LogDebug("Login Failed: Login Name is Required.");
                    return BadRequest("Login Name is Required.");
                }
                if (string.IsNullOrEmpty(authenticateRequestModel.Password))
                {
                    logger.LogDebug("Login Failed for {LoginName}: Password is Required.", LoginName);
                    return BadRequest("Password is Required.");
                }
            }

            try
            {
                authenticateRequestModel.SessionKey = Guid.NewGuid().ToString();
                if (appOptions.AppSettings.IsPasswordEncryptedInClient)
                {
                    EncryptDecryptAes.StrEncryptionKey = appOptions.AppSettings.EncryptionAlgorithmKey;
                    authenticateRequestModel.Password = EncryptDecryptAes.DecryptStringAES(authenticateRequestModel.Password);
                }

                authenticateResponseModel = AuthenticateUser(authenticateRequestModel);

            }
            catch(Exception ex)
            {

            }

            await Task.CompletedTask;

            return Ok(authenticateResponseModel);

        }

        private AuthenticateResponseModel? AuthenticateUser(AuthenticateRequestModel LoginModel, bool IsSso = false) 
        {
           
            AuthenticateResponseModel authenticateResponseModel = null;

            AuthenticateResponseModel user = authService.Authenticate(LoginModel, IPAddress(), IsSso);

            if(user != null)
            {
                if(user.Status == "N001")
                {
                    authenticateResponseModel = new AuthenticateResponseModel
                    {
                        Status = user.Status,
                        UserId = user.UserId,
                        LoginId = user.LoginId
                    };
                }
                else
                {
                    authenticateResponseModel = new AuthenticateResponseModel
                    {
                        IsFirstTimeLoggedIn = user.IsFirstTimeLoggedIn,
                        UserId = user.UserId,
                        LoginId = user.LoginId
                    };

                    Utilities.InsertStringCookie(HttpContext, "ACCESS-TOKEN", user.Token, JwtOptions.TokenValidityInMinutes);
                    Utilities.InsertStringCookie(HttpContext, "REFRESH-TOKEN", user.RefreshToken, JwtOptions.RefreshTokenValidityInMinutes);
                    Utilities.InsertStringCookie(HttpContext, "REF-TOKEN", user.RefKey, JwtOptions.RefreshTokenValidityInMinutes);
                    HttpContext.Response.Headers.Append("X-Token", user.SessionKey);

                    LoginModel.SessionKey = user.SessionKey;

                    authenticateResponseModel.RefreshInterval = TimeSpan.FromMinutes(JwtOptions.TokenValidityInMinutes).TotalSeconds;
                    authenticateResponseModel.Roles = user.Roles;
                    authenticateResponseModel.Status = user.Status;

                }
            }
            else
            {
                logger.LogDebug("Login Failed for { LoginName}: User not found.", LoginModel.LoginName);
            }
            return authenticateResponseModel;
        }

        private string IPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<AuthenticateResponseModel>> RefreshToken()
        {
            string Token = string.Empty;
            string RefreshToken = string.Empty;

            if (HttpContext.Request.Cookies.ContainsKey("REFRESH-TOKEN"))
            {
                RefreshToken = HttpContext.Request.Cookies["REFRESH-TOKEN"]?.Trim();
            }
            if (string.IsNullOrEmpty(RefreshToken))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            string csrftoken = null;

            if(appOptions.AppSettings.IsCsrfValidationEnabled && HttpContext.Request.Cookies.ContainsKey("X-XSRF-TOKEN"))
            {
                csrftoken = HttpContext.Request.Cookies["X-XSRF-TOKEN"].Trim();
            }

            AuthenticateResponseModel authenticateResponseModel = authService.RefreshToken(RefreshToken, Token, IPAddress(), csrftoken);

            if(authenticateResponseModel == null) return Unauthorized(new { message = "Invalid token" });

            AuthenticateResponseModel tokenResponseModel = new()
            {
                RefreshInterval = TimeSpan.FromMinutes(JwtOptions.TokenValidityInMinutes).TotalMinutes,
                Roles = authenticateResponseModel.Roles
            };

            Utilities.InsertStringCookie(HttpContext, "ACCESS-TOKEN", authenticateResponseModel.Token, JwtOptions.TokenValidityInMinutes);
            Utilities.InsertStringCookie(HttpContext, "REFRESH-TOKEN", authenticateResponseModel.RefreshToken, JwtOptions.RefreshTokenValidityInMinutes);
            Utilities.InsertStringCookie(HttpContext, "REF-TOKEN", authenticateResponseModel.RefKey, JwtOptions.RefreshTokenValidityInMinutes);

            await Task.Yield();
            return Ok(tokenResponseModel);
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = GetCurrentUserId(); // You already extract this in your token pipeline
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                    return BadRequest("No refresh token found.");

                var tokenEntry = await context.UserLoginTokens
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.RefreshToken == refreshToken);

                if (tokenEntry != null)
                {
                    context.UserLoginTokens.Remove(tokenEntry);
                    await context.SaveChangesAsync();
                }

                // Optional: remove cookie
                Response.Cookies.Delete("refreshToken");

                return Ok("Logout successful.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Logout failed");
                return StatusCode(500, "An error occurred while logging out.");
            }
        }

    }
}
