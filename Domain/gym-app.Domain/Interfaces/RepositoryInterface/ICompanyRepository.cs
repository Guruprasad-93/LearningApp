using gym_app.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Interfaces.RepositoryInterface
{
    public interface ICompanyRepository
    {
        Task<string> CompanyCreationUpdation(Company company);
        Task<List<Company>> GetCompanyDetails();
    }
}
