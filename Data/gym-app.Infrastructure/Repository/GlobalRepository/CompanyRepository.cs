using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Interfaces.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Infrastructure.Repository.GlobalRepository
{
    public class CompanyRepository : BaseRepository<CompanyRepository>, ICompanyRepository
    {
        private readonly ApplicationDbContext context;
        public AppOptions appOptions { get; set; }

        public CompanyRepository(ApplicationDbContext _context, ILogger<CompanyRepository> _logger, AppOptions _appOptions) : base(_logger)
        {
            context = _context;
            appOptions = _appOptions;
        }

        public async Task<string> CompanyCreationUpdation(Company company)
        {
            string Result = "E001";
            try
            {
                logger.LogDebug("CompanyRepository > CompanyCreationUpdation() started {model}", company);

                if(context.Companies.Any(c => c.CompanyId == company.CompanyId))
                {
                    var comp = context.Companies.Where(c => c.CompanyId == company.CompanyId).FirstOrDefault();
                    if(comp != null)
                    {
                        comp.CompanyName = company.CompanyName;
                        comp.CompOwnerName = company.CompOwnerName;
                        comp.Address = company.Address;
                        comp.PhotoPath = company.PhotoPath;
                        company.Modifiedby = company.Modifiedby;
                        company.ModifiedDate = DateTime.Now;
                        context.Update(comp);
                        await context.SaveChangesAsync();
                        Result = "U001";
                    }
                   
                }
                if(company.CompanyId == 0)
                {
                    company.Createdby = company.Createdby;
                    company.CreatedDate = DateTime.Now;
                    context.Companies.Add(company);
                    await context.SaveChangesAsync();
                    Result = "S001";
                }

                logger.LogDebug("CompanyRepository > CompanyCreationUpdation() completed {model}", company);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyRepository > CompanyCreationUpdation() {model}", company);
                throw;
            }

            return Result;
        }

        public async Task<List<Company>> GetCompanyDetails()
        {
            List<Company> Company = new List<Company>();
            try
            {
                logger.LogDebug("CompanyRepository > GetCompanyDetails() started");

                Company = await context.Companies.ToListAsync();

                logger.LogDebug("CompanyRepository > GetCompanyDetails() completed");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyRepository > GetCompanyDetails()");
                throw;
            }

            return Company;
        }
    }
}
