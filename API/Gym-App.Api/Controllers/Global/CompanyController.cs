using Gym_App.Api.Controllers.User;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using Microsoft.AspNetCore.Mvc;
using gym_app.Business.UserService;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.User;
using gym_app.Domain.Entities;

namespace Gym_App.Api.Controllers.Global
{
    public class CompanyController : BaseApiController<CompanyController>
    {
        private readonly JwtOptions JwtOptions;
        private readonly ICompanyService companyService;
        public CompanyController(ICompanyService _companyService, ILogger<CompanyController> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            companyService = _companyService;
            JwtOptions = appOptions.JwtOptions;
        }

        [HttpPost]
        [Route("companycreation")]
        public async Task<IActionResult> CompanyCreationUpdation([FromBody] Company company)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("CompanyController > CompanyCreationUpdation() started {model}", company);
                company.Createdby = GetCurrentUserId();
                company.CompanyId = 0;
                Result = await companyService.CompanyCreationUpdation(company);

                logger.LogDebug("CompanyController > CompanyCreationUpdation() completed {model}", company);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyController > CompanyCreationUpdation() {model}", company);
                throw;
            }

            return Ok(Result);
        }

        [HttpPost]
        [Route("companyupdation")]
        public async Task<IActionResult> CompanyUpdation([FromBody] Company company)
        {
            string Result = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                logger.LogDebug("CompanyController > CompanyUpdation() started {model}", company);
                company.Modifiedby = GetCurrentUserId();

                Result = await companyService.CompanyCreationUpdation(company);

                logger.LogDebug("CompanyController > CompanyUpdation() completed {model}", company);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyController > CompanyUpdation() {model}", company);
                throw;
            }

            return Ok(Result);
        }


        [HttpGet]
        [Route("companydetails")]
        public async Task<List<Company>> GetCompanyDetails()
        {
            List<Company> ltCompany = new List<Company>();

            try
            {
                logger.LogDebug("CompanyController > GetCompanyDetails() started");

                long cid = GetCurrentCompanyId();

                ltCompany = await companyService.GetCompanyDetails();

                logger.LogDebug("CompanyController > GetCompanyDetails() completed ");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyController > GetCompanyDetails() ");
                throw;
            }

            return ltCompany;
        }
    }
}
