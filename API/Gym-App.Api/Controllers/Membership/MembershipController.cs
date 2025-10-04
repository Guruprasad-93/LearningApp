using Gym_App.Api.Controllers.User;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using Microsoft.AspNetCore.Mvc;
using gym_app.Business.UserService;
using gym_app.Domain.Model.User;
using gym_app.Domain.Model.MembershipModel;
using gym_app.Domain.Model.CommonModel;
using Microsoft.AspNetCore.Authorization;

namespace Gym_App.Api.Controllers.Membership
{
    [ApiController]
    public class MembershipController : BaseApiController<MembershipController>
    {
        private readonly JwtOptions JwtOptions;
        private readonly IMembershipService memberShipService;
        public MembershipController(IMembershipService _memberShipService, ILogger<MembershipController> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            memberShipService = _memberShipService;
            JwtOptions = appOptions.JwtOptions;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("membershipcreation")]
        public async Task<IActionResult> MemberShipCreation([FromBody] MembershipModel membershipModel)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("MembershipController > MemberShipCreation() started {model}", membershipModel);
                var cid = GetCurrentCompanyId();

                Result = await memberShipService.MemberShipCreation(membershipModel);

                logger.LogDebug("MembershipController > MemberShipCreation() completed {model}", membershipModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in MembershipController > MemberShipCreation() {model}", membershipModel);
                throw;
            }

            return Ok(Result);
        }

        [HttpPost]
        [Route("membershipdetails")]
        public async Task<List<MemberShipDetails>> MemberShipDetails()
        {
            List<MemberShipDetails> memberShipDetails = new List<MemberShipDetails>();
            CommonModelClass commonModelClass = new CommonModelClass();

            commonModelClass.CompanyId = GetCurrentCompanyId();

            try
            {
                logger.LogDebug("MembershipController > MemberShipDetails() started {model}", commonModelClass);
               

                memberShipDetails = await memberShipService.MemberShipDetails(commonModelClass);

                logger.LogDebug("MembershipController > MemberShipDetails() completed {model}", commonModelClass);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in MembershipController > MemberShipDetails() {model}", commonModelClass);
                throw;
            }

            return memberShipDetails;
        }

    }
}
