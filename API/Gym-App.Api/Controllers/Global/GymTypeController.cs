using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Entities;
using gym_app.Domain.Interfaces.BusinessInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gym_app.Business.Global;

namespace Gym_App.Api.Controllers.Global
{
    public class GymTypeController : BaseApiController<GymTypeController>
    {
        private readonly JwtOptions JwtOptions;
        private readonly IGymTypeService gymTypeService;
        public GymTypeController(IGymTypeService _gymTypeService, ILogger<GymTypeController> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            gymTypeService = _gymTypeService;
            JwtOptions = appOptions.JwtOptions;
        }

        [HttpPost]
        [Route("gymtypecreation")]
        public async Task<IActionResult> GymTypeCreation(GymType gymType)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("GymTypeController > GymTypeCreation() started {model}", gymType);

                gymType.TypeId = 0;
                gymType.CreatedBy = GetCurrentUserId();

                Result = await gymTypeService.GymTypeCreation(gymType);

                logger.LogDebug("GymTypeController > GymTypeCreation() completed {model}", gymType);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GymTypeController > GymTypeCreation() {model}", gymType);
                throw;
            }

            return Ok(Result);
        }

        [HttpPost]
        [Route("gymtypeupdation")]
        public async Task<IActionResult> GymTypeUpdation(GymType gymType)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("GymTypeController > GymTypeUpdation() started {model}", gymType);
                gymType.ModifiedBy = GetCurrentUserId();
                Result = await gymTypeService.GymTypeCreation(gymType);

                logger.LogDebug("GymTypeController > GymTypeUpdation() completed {model}", gymType);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GymTypeController > GymTypeUpdation() {model}", gymType);
                throw;
            }

            return Ok(Result);
        }


        [HttpGet]
        [Route("gymtypedetails")]
        public async Task<List<GymType>> GetGymTypeDetails()
        {
            List<GymType> ltGymType = new List<GymType>();

            try
            {
                logger.LogDebug("GymTypeController > GetGymTypeDetails() started");

                ltGymType = await gymTypeService.GetGymTypeDetails();

                logger.LogDebug("GymTypeController > GetGymTypeDetails() completed ");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GymTypeController > GetGymTypeDetails() ");
                throw;
            }

            return ltGymType;
        }
    }
}
