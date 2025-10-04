using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.CustomerReview;
using gym_app.Domain.Model.User;
using Gym_App.Api.Controllers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym_App.Api.Controllers.User
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : BaseApiController<UserController>
    {
        private readonly IWebHostEnvironment _env;
        private readonly JwtOptions JwtOptions;
        private readonly IUserService userService;
        public UserController(IUserService _userService, ILogger<UserController> _logger, AppOptions appOptions, IWebHostEnvironment env) : base(appOptions, _logger)
        {
            userService = _userService;
            JwtOptions = appOptions.JwtOptions;
            _env = env;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("usercreation")]
        public async Task<IActionResult> UserCreationUpdation([FromForm] UserDetailsModel userDetailsModel)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("UserController > UserCreationUpdation() started {model}", userDetailsModel);
                userDetailsModel.CompanyId = GetCurrentCompanyId();
                userDetailsModel.UserId = GetCurrentUserId();


                Result = await userService.UserCreationUpdation(userDetailsModel, _env.WebRootPath);

                logger.LogDebug("UserController > UserCreationUpdation() completed {model}", userDetailsModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserController > UserCreationUpdation() {model}", userDetailsModel);
                throw;
            }

            return Ok(Result);
        }


        [HttpGet]
        [Route("userdetails")]
        public async Task<List<UserDetails>> GetUserDetails()
        {
            List<UserDetails> ltUserDetails = new List<UserDetails>();
            CommonModelClass commonModelClass = new CommonModelClass();

            try
            {
                logger.LogDebug("UserController > GetUserDetails() started {model}", commonModelClass);

                long cid = GetCurrentCompanyId();

                ltUserDetails = await userService.GetUserDetails(commonModelClass);

                logger.LogDebug("UserController > GetUserDetails() completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserController > GetUserDetails() {model}", commonModelClass);
                throw;
            }

            return ltUserDetails;
        }

        [HttpGet]
        [Route("userprofile")]
        public async Task<UserProfile> GetUserProfile()
        {
            UserProfile ltUserProfile = new UserProfile();
            CommonModelClass commonModelClass = new CommonModelClass();

            try
            {
                logger.LogDebug("UserController > GetUserProfile() started {model}", commonModelClass);

                long cid = GetCurrentCompanyId();

                ltUserProfile = await userService.GetUserProfile(commonModelClass);

                logger.LogDebug("UserController > GetUserProfile() completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserController > GetUserProfile() {model}", commonModelClass);
                throw;
            }

            return ltUserProfile;
        }

    }
}
