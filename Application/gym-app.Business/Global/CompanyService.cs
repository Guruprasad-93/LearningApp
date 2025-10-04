using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.MembershipModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Business.Global
{
    public class CompanyService : BaseService<CompanyService>, ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly JwtOptions JwtOptions;

        public CompanyService(ICompanyRepository _companyRepository, AppOptions _appOptions, ILogger<CompanyService> _logger ) : base(_appOptions, _logger)
        {
            companyRepository = _companyRepository;
            JwtOptions = appOptions.JwtOptions;
        }

        public async Task<string> CompanyCreationUpdation(Company company)
        {
            string Result = "";
            try
            {
                logger.LogDebug("CompanyService > CompanyCreationUpdation() started {model}", company);

                Result = await companyRepository.CompanyCreationUpdation(company);

                logger.LogDebug("CompanyService > CompanyCreationUpdation() completed {model}", company);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyService > CompanyCreationUpdation() {model}", company);
                throw;
            }

            return Result;
        }

        public async Task<List<Company>> GetCompanyDetails()
        {
            List<Company> Company = new List<Company>();
            try
            {
                logger.LogDebug("CompanyService > GetCompanyDetails() started");

                Company = await companyRepository.GetCompanyDetails();

                logger.LogDebug("CompanyService > GetCompanyDetails() completed");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyService > GetCompanyDetails()");
                throw;
            }

            return Company;
        }
    }
}
