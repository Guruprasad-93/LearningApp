using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Entities;
using gym_app.Domain.Interfaces.BusinessInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Gym_App.Api.Controllers.Global
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RoleController : BaseApiController<RoleController>
    {
        private readonly JwtOptions JwtOptions;
        private readonly IRoleService roleService;
        public RoleController(IRoleService _roleService, ILogger<RoleController> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            roleService = _roleService;
            JwtOptions = appOptions.JwtOptions;
        }

        [HttpPost]
        [Route("rolecreation")]
        public async Task<IActionResult> RoleCreationUpdation(Role role)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("RoleController > RoleCreationUpdation() started {model}", role);

                role.RoleId = 0;
                role.CreatedBy = GetCurrentUserId();

                Result = await roleService.RoleCreationUpdation(role);

                logger.LogDebug("RoleController > RoleCreationUpdation() completed {model}", role);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RoleController > RoleCreationUpdation() {model}", role);
                throw;
            }

            return Ok(Result);
        }

        [HttpPost]
        [Route("roleupdate")]
        public async Task<IActionResult> RoleUpdation(Role role)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("RoleController > RoleUpdation() started {model}", role);

                role.ModifiedBy = GetCurrentUserId();

                Result = await roleService.RoleCreationUpdation(role);

                logger.LogDebug("RoleController > RoleUpdation() completed {model}", role);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RoleController > RoleUpdation() {model}", role);
                throw;
            }

            return Ok(Result);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("roledetails")]
        public async Task<List<Role>> GetRoleDetails()
        {
            List<Role> ltRole = new List<Role>();

            try
            {
                logger.LogDebug("RoleController > GetRoleDetails() started");

                ltRole = await roleService.GetRoleDetails();

                logger.LogDebug("RoleController > GetRoleDetails() completed ");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RoleController > GetRoleDetails() ");
                throw;
            }

            return ltRole;
        }
    }
}
