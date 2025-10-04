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
    public class RoleRepository : BaseRepository<RoleRepository>, IRoleRepository
    {
        private readonly ApplicationDbContext context;
        public AppOptions appOptions { get; set; }
        public RoleRepository(ApplicationDbContext _context, ILogger<RoleRepository> _logger, AppOptions _appOptions) : base(_logger)
        {
            context = _context;
            appOptions = _appOptions;
        }

        public async Task<List<Role>> GetRoleDetails()
        {
            List<Role> Role = new List<Role>();
            try
            {
                logger.LogDebug("RoleRepository > GetRoleDetails() started");

                Role = await context.Roles.ToListAsync();

                logger.LogDebug("RoleRepository > GetRoleDetails() completed");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RoleRepository > GetRoleDetails()");
                throw;
            }

            return Role;
        }

        public async Task<string> RoleCreationUpdation(Role role)
        {
            string Result = "E001";
            try
            {
                logger.LogDebug("RoleRepository > RoleCreationUpdation() started {model}", role);

                if (context.Roles.Any(r => r.RoleId == role.RoleId))
                {
                    var roledetails = context.Roles.Where(r => r.RoleId == role.RoleId).FirstOrDefault();
                    if (roledetails != null)
                    {
                        roledetails.RoleCode = role.RoleCode;
                        roledetails.RoleName = role.RoleName;
                        roledetails.RoleLevel = role.RoleLevel;
                        roledetails.ModifiedBy = role.ModifiedBy;
                        roledetails.ModifiedDate = DateTime.Now;

                        context.Update(roledetails);
                        await context.SaveChangesAsync();
                        Result = "U001";
                    }

                }
                if (role.RoleId == 0)
                {
                    role.CreatedDate = DateTime.Now;
                    role.CreatedBy = role.CreatedBy;
                    context.Roles.Add(role);
                    await context.SaveChangesAsync();
                    Result = "S001";
                }

                logger.LogDebug("RoleRepository > RoleCreationUpdation() completed {model}", role);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RoleRepository > RoleCreationUpdation() {model}", role);
                throw;
            }

            return Result;
        }
    }
}
